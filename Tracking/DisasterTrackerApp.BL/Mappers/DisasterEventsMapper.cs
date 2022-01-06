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
                    Properties = MapPropertyDtoToEntity(disasterEvent.Properties),
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
        public static DisasterPropertyEntity MapPropertyDtoToEntity(PropertyDto propertyDto)
        {
            return new DisasterPropertyEntity()
            {
                Title = propertyDto.Title,
                Closed = propertyDto.Closed,
                //Sources = propertyDto.Sources.Select(MapSourceDtoToEntity).ToList(), //TODO: check events on non-existent JSON property sources property 
                Categories = propertyDto.Categories.Select(MapCategoryDtoToEntity).ToList()
            };
        }
    }
}
