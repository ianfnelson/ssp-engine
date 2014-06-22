using System;
using Autofac;

namespace SspEngineClient
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // setup log4net

            AutoMapperConfiguration.Configure();

            var container = AutofacConfiguration.Configure();

            container.Resolve<ICore>().RateRisks();

            Console.WriteLine("(hit a key to exit)");
            Console.ReadKey();
        }
    }
}
