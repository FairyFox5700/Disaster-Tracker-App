using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.Entities;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Mappers;
using DisasterTrackerApp.BL.HttpClients.Contract;

namespace DisasterTrackerApp.BL.Implementation
{
    public class NewClosedEventsService : INewClosedEventsService
    {
        private readonly IClosedDisasterEventsClient _client;
        private readonly IDisasterEventRepository _disasterEventRepository;
        public NewClosedEventsService(IClosedDisasterEventsClient client, IDisasterEventRepository disasterEventRepository)
        {
            _client = client;
            _disasterEventRepository = disasterEventRepository;
        }
        public async Task AddNewClosedEvents(CancellationToken cancellationToken)
        {
            var events = await _client.GetDisasterEventsAsync(cancellationToken).ConfigureAwait(false);
            var mappedEvents = events.Data.Select(DisasterEventsMapper.MapDisasterEventDtoToEntity).ToList();
            var lastEvent = await _disasterEventRepository.GetLastDisasterEventByClosedTime().ConfigureAwait(false);
            if (lastEvent != null)
            {
                await _disasterEventRepository.AddEvents(mappedEvents.Where(x => x.Closed > lastEvent.Closed).ToList()).ConfigureAwait(false);
            }
            else
            {
                await _disasterEventRepository.AddEvents(mappedEvents).ConfigureAwait(false);
            }
        }
    }

}