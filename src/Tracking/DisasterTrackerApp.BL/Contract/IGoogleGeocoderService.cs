using NetTopologySuite.Geometries;

namespace DisasterTrackerApp.BL.Contract;

public interface IGoogleGeocoderService
{
    Task<Point?> GetLocationCoordinates(string location);
}