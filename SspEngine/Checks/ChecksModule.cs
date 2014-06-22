using Autofac;

namespace SspEngine.Checks
{
    public class ChecksModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostcodeToGeoCoordinateService>().As<IPostcodeToGeoCoordinateService>();
            builder.RegisterType<PostcodeToGeoCoordinateServiceFactory>().As<IPostcodeToGeoCoordinateServiceFactory>();
            builder.RegisterType<OccupationCheck>().As<ICheck>();
            builder.RegisterType<VehicleKeptCheck>().As<ICheck>();
        }
    }
}