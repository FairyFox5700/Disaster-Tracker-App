using System.Reactive.Linq;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Mappers.Contract;
using DisasterTrackerApp.BL.Mappers.Implementation;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Logging;

namespace DisasterTrackerApp.BL.Implementation;

public class UsersGoogleCalendarDataUpdatingService : IUsersGoogleCalendarDataUpdatingService
{
    private readonly ILogger<UsersGoogleCalendarDataUpdatingService> _logger;
    private readonly ICalendarRepository _calendarRepository;
    private readonly ICalendarEventsRepository _calendarEventsRepository;
    private readonly IGoogleUserRepository _userRepository;
    private readonly ICalendarEventMapper _eventConverter;
    private readonly IGoogleCalendarService _calendarService;
    private readonly IRedisWatchChannelsRepository _watchChannelsRepository;

    public UsersGoogleCalendarDataUpdatingService(
        ILogger<UsersGoogleCalendarDataUpdatingService> logger,
        ICalendarRepository calendarRepository, 
        ICalendarEventsRepository calendarEventsRepository, 
        ICalendarEventMapper eventConverter,
        IGoogleUserRepository userRepository, 
        IGoogleCalendarService calendarService,
        IRedisWatchChannelsRepository watchChannelsRepository)
    {
        _logger = logger;
        _calendarRepository = calendarRepository;
        _calendarEventsRepository = calendarEventsRepository;
        _eventConverter = eventConverter;
        _userRepository = userRepository;
        _calendarService = calendarService;
        _watchChannelsRepository = watchChannelsRepository;
    }

    public async Task UpdateUserData(GoogleUser user)
    {
        var primaryCalendar = await UpdateUserCalendar(user.UserId);
        if (primaryCalendar == null)
        {
            return;
        }

        var events = await _calendarService.GetUserEventsAsync(user.UserId, primaryCalendar.GoogleCalendarId, user.LastVisit);
        var success = await UpdateCalendarEvents(primaryCalendar.Id, events);

        if (success)
        {
            _logger.LogInformation("User's primary calendar and it's events were successfully updated");
            await _userRepository.SetLastLoginDataUpdateDateTime(user);
        }
    }

    public async Task<string?> RegisterWatch(Guid userId)
    {
        var calendars = await _calendarRepository.GetFilteredAsync(c => c.UserId == userId);
        var primaryCalendar = calendars.FirstOrDefault(c => c.Primary.HasValue && c.Primary.Value);
        if (primaryCalendar == null)
        {
            _logger.LogWarning("Cannot find primary calendar for user {Id}", userId.ToString());
            return null;
        }

        return await CreateOrUpdateWatchChannel(userId, primaryCalendar.GoogleCalendarId);
    }

    private async Task<string?> CreateOrUpdateWatchChannel(Guid userId, string googleCalendarId)
    {
        var existingChannel = _watchChannelsRepository.GetWatchChannel(userId);
        if (existingChannel == null)
        {
            var channel = await _calendarService.WatchEvents(userId, googleCalendarId);
            _logger.LogInformation("Watch channel for user {Id} primary calendar was created", userId);
            return channel.Token;
        }

        await _calendarService.StopWatchEvents(existingChannel.ChannelToken);
        var newWatchChannel = await _calendarService.WatchEvents(userId, googleCalendarId);
        _logger.LogInformation("Watch channel for user {Id} primary calendar was updated", userId);

        return newWatchChannel.Token;
    }

    public async Task<bool> StopWatch(string channelToken)
    {
        var success = await _calendarService.StopWatchEvents(channelToken);
        _logger.LogInformation("Trying to stop watch channel with token {Token}. Success: {Result}", channelToken, success);
        
        return success;
    }

    public async Task UpdateCalendarEventsOnWebHook(string channelToken, DateTime triggerTime)
    {
        _logger.LogInformation("Updating user's events after web hook trigger. Channel token: {Token}", channelToken);
        var watchData = WatchChannelDataMapper.MapChannelDataEntityToDto(_watchChannelsRepository.GetWatchChannel(channelToken));
        if (watchData == null)
        {
            _logger.LogWarning("Cannot find watch channel by token {Token}", channelToken);
            return;
        }
        
        var calendar = (await _calendarRepository.GetFilteredAsync(c => c.UserId == watchData.UserId))
                        .FirstOrDefault();
        
        if (calendar == null)
        {
            _logger.LogWarning("Cannot find primary calendar for user {Id}" +
                               "Unable to process watched resource notification. Token: {Token}", 
                      watchData.UserId.ToString(), channelToken);
            return;
        }
        
        var events = await _calendarService.GetUserEventsAsync(watchData.UserId, calendar.GoogleCalendarId, watchData.LastTimeTriggered);
        var success = await UpdateCalendarEvents(calendar.Id, events);
        
        _logger.LogInformation("Trying to process watch channel {Token} web hook trigger. Success: {Success}", channelToken, success);
        
        _watchChannelsRepository.Save(channelToken, WatchChannelDataMapper.MapChannelDataDtoToEntity(watchData.UpdateTriggerTime(triggerTime)));
    }

    private async Task<GoogleCalendar?> UpdateUserCalendar(Guid userId)
    {
        var primaryCalendar = await _calendarService.GetUserCalendarAsync(userId);
        if (primaryCalendar == null)
        {
            _logger.LogWarning("Cannot find primary calendar for user {Id}", userId.ToString());
            
            return null;
        }
        
        var existingUserCalendar = (await _calendarRepository.GetFilteredAsync(
            c => c.GoogleCalendarId == primaryCalendar.GoogleCalendarId)).FirstOrDefault();

        if (existingUserCalendar != null)
        {
            await _calendarRepository.UpdateAsync(existingUserCalendar, primaryCalendar);
            return existingUserCalendar;
        }
        
        await _calendarRepository.SaveAsync(primaryCalendar);
        
        return primaryCalendar;
    }

    private async Task<bool> UpdateCalendarEvents(Guid calendarId, IEnumerable<Event> calendarEvents)
    {
        try
        {
            var changedCalendarEvents =calendarEvents.ToList();
            var existingCalendarEvents = await _calendarEventsRepository
                .GetFilteredAsync(e => e.CalendarId == calendarId);
            
            var eventsToDelete = changedCalendarEvents
                .Select(e => e.Id)
                .Intersect(existingCalendarEvents.Select(e=>e.GoogleEventId))
                .Select(id => existingCalendarEvents.First(e => e.GoogleEventId == id));

            await _calendarEventsRepository.RemoveUserEventsAsync(eventsToDelete);

            await RemoveOutdatedCalendarEvents(existingCalendarEvents);

            var updatedEvents = await Task.WhenAll(changedCalendarEvents
                .Where(e => e.Status != "cancelled")
                .Select(async e => await _eventConverter.ToCalendarEventEntity(e, calendarId)));

            await _calendarEventsRepository.SaveRangeAsync(updatedEvents);

            return true;
        }
        catch (Exception exception)
        {
            _logger.LogError("Error occured while trying to update user's data in database." +
                             "Exception message: {Message}", exception.Message);
            
            return false;
        }

    }

    private async Task RemoveOutdatedCalendarEvents(IEnumerable<CalendarEvent> calendarEvents)
    {
        var outdatedEvents = calendarEvents.Where(IsOutdated).ToList();
        _logger.LogInformation("{Count} outdated events were detected", outdatedEvents.Count);
        await _calendarEventsRepository.RemoveUserEventsAsync(outdatedEvents);
    }

    private bool IsOutdated(CalendarEvent @event)
    {
        return @event.EndTs < DateTime.UtcNow;
    }
}