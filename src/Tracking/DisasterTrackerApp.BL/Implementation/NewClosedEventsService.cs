using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Mappers;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.Mappers.Implementation;

namespace DisasterTrackerApp.BL.Implementation
{
    public class NewClosedEventsService : INewClosedEventsService
    {
        private readonly IClosedDisasterEventsClient _closedDisasterEventsClient;
        private readonly IDisasterEventRepository _disasterEventRepository;
        public NewClosedEventsService(IClosedDisasterEventsClient closedDisasterEventsClient,
            IDisasterEventRepository disasterEventRepository)
        {
            _closedDisasterEventsClient = closedDisasterEventsClient;
            _disasterEventRepository = disasterEventRepository;
        }
        public async Task AddNewClosedEvents(CancellationToken cancellationToken)
        {
            var events = await _closedDisasterEventsClient
                .GetDisasterEventsAsync(cancellationToken)
                .ConfigureAwait(false);
            var mappedEvents = events.Data
                .Select(DisasterEventsMapper.MapDisasterEventDtoToEntity)
                .ToList();
            var lastEvent = await _disasterEventRepository
                .GetLastDisasterEventByClosedTime()
                .ConfigureAwait(false);
            if (lastEvent == null)
            {
                await _disasterEventRepository.AddEvents(mappedEvents)
                    .ConfigureAwait(false);
            }
            else
            {
                await _disasterEventRepository
                    .AddEvents(mappedEvents
                        .Where(x => x.Closed > lastEvent.Closed).ToList())
                    .ConfigureAwait(false);
            }
        }
    }

}