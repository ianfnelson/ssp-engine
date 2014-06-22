using Autofac;

namespace SspEngine
{
    public class SspEngineModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Engine>().As<IEngine>();
        }
    }
}