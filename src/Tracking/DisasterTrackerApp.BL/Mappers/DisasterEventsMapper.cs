using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Disaster;

namespace DisasterTrackerApp.BL.Mappers
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
                    //Sources = disasterEvent.Properties.Sources.Select(MapSourceDtoToEntity).ToList(), //TODO: check events on non-existent JSON property sources property 
                    Categories = disasterEvent.Properties.Categories.Select(MapCategoryDtoToEntity).ToList(),
                    Geometry = disasterEvent.Geometry
                };
        }
        public static SourceEntity MapSourceDtoToEntity(SourceDto sourceDto)
        {
            return
                new SourceEntity()
                {
                    ExternalApiId = sourceDto.Id,
                    Url = sourceDto.Url
                };
        }
        public static CategoryEntity MapCategoryDtoToEntity(CategoryDto categoryDto)
        {
            return
                new CategoryEntity()
                {
                    ExternalApiId = categoryDto.Id,
                    Title = categoryDto.Title
                };
        }
    }
}
