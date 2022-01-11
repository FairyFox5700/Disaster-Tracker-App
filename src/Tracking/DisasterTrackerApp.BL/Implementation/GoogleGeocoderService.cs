using DisasterTrackerApp.BL.Contract;
using Geocoding;
using Geocoding.Google;
using NetTopologySuite.Geometries;

namespace DisasterTrackerApp.BL.Implementation;

public class GoogleGeocoderService : IGoogleGeocoderService
{
    private const string ApiKey = "AIzaSyCjOhCGWaKhLX0T46BUZCAZPMv5_2EPei8";
    
    public async Task<Point?> GetLocationCoordinates(string location)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            return default;
        }
        IGeocoder geocoder = new GoogleGeocoder { ApiKey = ApiKey };
        IEnumerable<Address> addresses = await geocoder.GeocodeAsync(location);
        var addressCoordinates = addresses.FirstOrDefault()?.Coordinates;
        
        return addressCoordinates == null 
            ? default 
            : new Point(addressCoordinates.Longitude, addressCoordinates.Latitude);
    }
}