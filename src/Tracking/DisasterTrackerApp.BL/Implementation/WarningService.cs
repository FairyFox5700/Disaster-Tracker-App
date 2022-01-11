
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.Internal;
using DisasterTrackerApp.BL.Mappers.Implementation;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Warnings;
using GeoCoordinatePortable;

namespace DisasterTrackerApp.BL.Implementation
{
    public class WarningService : IWarningService
    {
        private readonly IDisasterEventsClient _disasterEventsClient;
        private readonly IRedisDisasterEventsRepository _redisDisasterEventsRepository;
        private readonly ICalendarEventsRepository _calendarEventsRepository;
        private readonly IDisasterEventRepository _disasterEventRepository;
        private const int MaxRadiusInMeters = 100000;

        public WarningService(IDisasterEventsClient disasterEventsClient,
            IRedisDisasterEventsRepository redisDisasterEventsRepository,
            IDisasterEventRepository disasterEventRepository,
            ICalendarEventsRepository calendarEventsRepository)
        {
            _disasterEventsClient = disasterEventsClient;
            _redisDisasterEventsRepository = redisDisasterEventsRepository;
            _disasterEventRepository = disasterEventRepository;
            _calendarEventsRepository = calendarEventsRepository;
            FetchNewDisasterEvents(CancellationToken.None);
        }

        public IObservable<WarningDto> GetWarningEvents(WarningRequest warningRequest,
            CancellationToken cancellationToken = default)
        {
            return from c in _calendarEventsRepository.GetFilteredStreamAsync(BuildExpression(warningRequest))
                    .SelectMany(e => e)
                    .Do(e => Console.WriteLine($"{e.Id}+{e.Coordinates?.X}+{e.Coordinates?.Y}"))
                from d in _redisDisasterEventsRepository.GetAllDisasterEvents().SelectMany(e => e)
                where d.Geometry.Coordinates.Select(e => new GeoCoordinate(e.Y, e.X))
                    .Any(e => e.GetDistanceTo(new GeoCoordinate(c.Coordinates?.Y ?? 0, c.Coordinates?.X ?? 0)) <
                              MaxRadiusInMeters)
                select new WarningDto(c.Id,
                    d.Id,
                    $"Warning. There is an active disaster '{d.Title}' near your event location '{c.Location}'.",
                    c.EndTs,
                    c.StartedTs
                );
        }

        public IObservable<WarningDto> GetStatisticsWarningEvents(WarningRequest warningRequest,
            CancellationToken cancellationToken = default)
        {
            return
                _disasterEventRepository.GetDisasterEventsByCalendarInRadius(BuildExpression(warningRequest),
                        MaxRadiusInMeters)
                    .SelectMany(e => e)
                    .Distinct(d => d.Item2.Title)
                    .Select(e => new WarningDto(e.Item1.Id,
                        e.Item2.Id,
                        $"Warning. Disaster '{e.Item2.Title}' may occur near your event location '{e.Item1.Location}'",
                        e.Item1.EndTs,
                        e.Item1.StartedTs));
        }

        private void FetchNewDisasterEvents(CancellationToken cancellationToken)
        {
            _disasterEventsClient.GetDisasterEventsAsync(cancellationToken)
                .Select(e =>
                {
                    _redisDisasterEventsRepository.GetDisasterEventById(e.Properties.Id)
                        .IsEmpty()
                        .Do(empty =>
                        {
                            if (empty)
                            {
                                _redisDisasterEventsRepository.CreateDisasterEvent(DisasterEventsMapper
                                    .MapDisasterEventDtoToEntity(e));
                            }
                        })
                        .Subscribe();
                    return e;
                })
                .DistinctUntilChanged(e => e.Properties.Id)
                .SubscribeOn(NewThreadScheduler.Default)
                .LogWithThread()
                .RetryWithBackoffStrategy()
                .Subscribe();
        }
        
        #region private_members
        private Expression<Func<CalendarEvent, bool>> BuildExpression(WarningRequest warningRequest)
        {
            if(warningRequest.StartDateTimeOffset != null && warningRequest.EndDateTimeOffset != null)
            {
                return date => date.StartedTs <= warningRequest.StartDateTimeOffset && date.EndTs >= warningRequest.EndDateTimeOffset;
            }

            if (warningRequest.StartDateTimeOffset != null)
            {
                return date => date.StartedTs <= warningRequest.StartDateTimeOffset;
            }

            if (warningRequest.EndDateTimeOffset != null)
            {
                return date => date.EndTs >= warningRequest.EndDateTimeOffset;
            }

            return data => true;
        }

        #endregion
    }
}