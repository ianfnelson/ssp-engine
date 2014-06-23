using System;
using Autofac;
using log4net.Config;

namespace SspEngineClient
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // log4net
            XmlConfigurator.Configure();

            // AutoMapper
            AutoMapperConfiguration.Configure();

            // Autofac
            var container = AutofacConfiguration.Configure();

            // Let's rate some risks!
            container.Resolve<ICore>().RateRisks();

            Console.ReadKey();
        }
    }
}
