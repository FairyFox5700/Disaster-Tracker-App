using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.Internal;
using DisasterTrackerApp.BL.Mappers;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Warnings;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace DisasterTrackerApp.BL.Implementation
{
    public class WarningService : IWarningService
    {
        private readonly IDisasterEventsClient _disasterEventsClient;
        private readonly IRedisDisasterEventsRepository _redisDisasterEventsRepository;
        private readonly ICalendarRepository _calendarRepository;
        private const int MaxRadius = 40;
    
        public WarningService(IDisasterEventsClient disasterEventsClient,
            IRedisDisasterEventsRepository redisDisasterEventsRepository,
            ICalendarRepository calendarRepository)
        {
            _disasterEventsClient = disasterEventsClient;
            _redisDisasterEventsRepository = redisDisasterEventsRepository;
            _calendarRepository = calendarRepository;
            FetchNewDisasterEvents(CancellationToken.None);
        }
        public IObservable<WarningDto> GetWarningEvents(WarningRequest warningRequest,
            CancellationToken cancellationToken = default)
        {
            return Observable.FromAsync(async () =>
                from c in await _calendarRepository.GetCalendarEventsFilteredWithUserId(warningRequest.UserId,
                    BuildExpression(warningRequest),
                    cancellationToken)
                from d in _redisDisasterEventsRepository.GetAllDisasterEvents()
                where d.Geometry.IsWithinDistance((Geometry)c.Coordiantes,MaxRadius)
                    select new WarningDto(c.Id,
                        d.Id, 
                        $"Warning. Disaster can occur near your event in place {c.Location}",
                              c.EndTs,
                          c.StartedTs
                    ))
                .SelectMany(e=>e);
        }

        private void FetchNewDisasterEvents(CancellationToken cancellationToken)
        {
            _disasterEventsClient.GetDisasterEventsAsync(cancellationToken)
                .Do(e =>
                {
                    if (_redisDisasterEventsRepository.GetDisasterEventById(e.Properties.Id) == null)
                    {
                        _redisDisasterEventsRepository.CreateDisasterEvent(DisasterEventsMapper
                            .MapDisasterEventDtoToEntity(e));
                    }
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