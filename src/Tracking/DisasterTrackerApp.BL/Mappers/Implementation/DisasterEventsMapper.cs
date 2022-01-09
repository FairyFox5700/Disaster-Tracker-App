using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.BL.Mappers.Implementation
{
    public static class DisasterEventsMapper
    {
        public static DisasterEvent MapDisasterEventDtoToEntity(FeatureDto disasterEvent)
        {
            return
                new DisasterEvent()
                {
                    ExternalApiId = disasterEvent.Properties.Id,
                    Title = disasterEvent.Properties.Title,
                    Closed = disasterEvent.Properties.Closed,
                    Geometry = disasterEvent.Geometry,
                    Id = Guid.NewGuid(),
                };
        }
    }
}
