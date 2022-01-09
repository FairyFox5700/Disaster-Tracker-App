using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using DisasterTrackerApp.Dal.Repositories.Contract;
using DisasterTrackerApp.BL.Contract;
using DisasterTrackerApp.BL.Mappers;
using DisasterTrackerApp.BL.HttpClients.Contract;
using DisasterTrackerApp.BL.Mappers.Implementation;
using DisasterTrackerApp.Entities;

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

        public Task AddNewClosedEvents(CancellationToken cancellationToken)
        {
            _closedDisasterEventsClient.GetDisasterEventsAsync(cancellationToken)
                .SelectMany(e => e.Data)
                .Select(DisasterEventsMapper.MapDisasterEventDtoToEntity)
                .Let(mappedEvents => _disasterEventRepository.GetLastDisasterEventByClosedTime()
                    .Select(async lastEvent =>
                    {
                        if (lastEvent == null)
                        {
                            mappedEvents.Buffer(5)
                                .SelectMany(e =>
                                {
                                    _disasterEventRepository.AddEvents(e)
                                        .Subscribe(new Subject<Unit>());
                                    return e;
                                });
                        }
                        else
                        {
                            mappedEvents.Buffer(5)
                                .SelectMany(e =>
                                {
                                    _disasterEventRepository
                                        .AddEvents(e
                                            .Where(x => x.Closed > lastEvent.Closed))
                                        .Subscribe(new Subject<Unit>());
                                    return e;
                                });
                        }
                    }));
        return Task.CompletedTask;
        }
    }

}