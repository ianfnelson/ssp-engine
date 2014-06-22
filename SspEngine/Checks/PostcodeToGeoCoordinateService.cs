using System.Device.Location;
using EasyHttp.Http;
using JsonFx.Json;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public class PostcodeToGeoCoordinateService : IPostcodeToGeoCoordinateService
    {
        public GeoCoordinate GetCoordinatesForPostcode(Postcode postcode)
        {
            var http = new HttpClient();
            http.Request.Accept = HttpContentTypes.ApplicationJson;

            var url = BuildUrl(postcode);

            var response = http.Get(url);

            // HACK - have to override content type as service returns application/octet-stream !
            //        as a result, we also have to use StaticBody instead of sexier DynamicBody method. *sulk*
            var postcodeData = response.StaticBody<GeoServiceResponse>("application/json");     

            return new GeoCoordinate(postcodeData.Geo.Latitude, postcodeData.Geo.Longitude);
        }

        private string BuildUrl(Postcode postcode)
        {
            return string.Format("http://www.uk-postcodes.com/postcode/{0}{1}.json", postcode.OutCode, postcode.InCode);
        }

        /// <summary>
        /// *Grumble*
        /// </summary>
        public class GeoServiceResponse
        {
            [JsonName("geo")]
            public GeoData Geo { get; set; }

            [JsonName("geo")]
            public class GeoData
            {
                [JsonName("lat")]
                public double Latitude { get; set; }

                [JsonName("lng")]
                public double Longitude { get; set; }
            }
        }
    }
}