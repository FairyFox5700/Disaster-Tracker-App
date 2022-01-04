using DisasterTrackerApp.Entities;
using DisasterTrackerApp.Models.Disaster;
using NetTopologySuite.Geometries;

namespace DisasterTrackerApp.BL;

public static class DisasterEventsMapper
{
    public static DisasterEvent MapDisasterEventDtoToEntity(DisasterEventDto disasterEvent)
    {
        return
            new DisasterEvent()
            {
                Description = disasterEvent.Description ?? "",
                Active = disasterEvent.Closed != null,
                CategoryTittle = disasterEvent.Categories.First()?.Title,
                Coordiantes = disasterEvent.Geometry.Select(MapGeometryDtoToEntity).ToList(),
                ExternalApiId = disasterEvent.Id,
                Tittle = disasterEvent.Title,
                CreatedAt = DateTimeOffset.Now,
                Id = Guid.NewGuid(),
                UpdatedAt = DateTimeOffset.Now,
            };
    }

    public static DisasterEventGeometry MapGeometryDtoToEntity(GeometryDto geometryDto)
    {
        return new DisasterEventGeometry()
        {
            EventId = Guid.NewGuid(),
            Id = Guid.NewGuid(),
            Point = new Point(geometryDto.Coordinates[0], geometryDto.Coordinates[1]),
            Time = DateTime.Now,
        };
    }
}