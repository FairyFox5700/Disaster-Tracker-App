using DisasterTrackerApp.Dal.Repositories.Contract;
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
        public async Task AddNewClosedEvents()
        {
            var events = await _client.GetDisasterEventsAsync();
            var mappedEvents = events.Data.Select(DisasterEventsMapper.MapDisasterEventDtoToEntity).ToList();
            await _disasterEventRepository.AddExceptClosedDisasterEvents(mappedEvents);
        }
    }

}