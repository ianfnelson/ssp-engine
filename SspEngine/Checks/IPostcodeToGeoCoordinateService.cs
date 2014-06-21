using SspEngine.DomainModel;
using System.Device.Location;

namespace SspEngine.Checks
{
    public interface IPostcodeToGeoCoordinateService
    {
        GeoCoordinate GetCoordinatesForPostcode(Postcode postcode);
    }
}