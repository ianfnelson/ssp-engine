namespace SspEngine.Checks
{
    public class PostcodeToGeoCoordinateServiceFactory : IPostcodeToGeoCoordinateServiceFactory
    {
        public IPostcodeToGeoCoordinateService Create()
        {
            return new PostcodeToGeoCoordinateService();
        }
    }
}