using System;
using System.Device.Location;
using System.Threading.Tasks;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public class VehicleKeptCheck : ICheck
    {
        private readonly IPostcodeToGeoCoordinateServiceFactory _serviceFactory;

        public VehicleKeptCheck(IPostcodeToGeoCoordinateServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public string Description
        {
            get { return "Vehicle Kept Check"; }
        }

        public int Ordinality
        {
            get { return 2; }
        }

        public RatingResult RunCheck(Risk risk)
        {
            var coordinatePair = GetCoordinatePair(risk);

            var distance = coordinatePair.Item1.GetDistanceTo(coordinatePair.Item2);

            return InterpretDistance(distance);
        }

        private static RatingResult InterpretDistance(double distance)
        {
            if (distance < AppSettingsProvider.Current.VehicleKeptCheck_AcceptBelowMetres) return RatingResult.Accept;

            return distance < AppSettingsProvider.Current.VehicleKeptCheck_ReferBelowMetres ? RatingResult.Refer : RatingResult.Decline;
        }

        private Tuple<GeoCoordinate, GeoCoordinate> GetCoordinatePair(Risk risk)
        {
            GeoCoordinate addressCoordinates = null;
            GeoCoordinate keptCoordinates = null;

            var t1 =
                Task.Factory.StartNew(() => addressCoordinates = _serviceFactory.Create().GetCoordinatesForPostcode(risk.Address.Postcode));
            var t2 =
                Task.Factory.StartNew(() => keptCoordinates = _serviceFactory.Create().GetCoordinatesForPostcode(risk.KeptPostcode));

            Task.WaitAll(t1, t2);

            return new Tuple<GeoCoordinate, GeoCoordinate>(addressCoordinates, keptCoordinates);
        }

    }
}