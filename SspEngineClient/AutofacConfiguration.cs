using System;
using System.Reflection;
using Autofac;
using AutoMapper;
using SspEngine;

namespace SspEngineClient
{
    public static class AutofacConfiguration
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetAssembly(typeof (Engine)));

            builder.RegisterType<Core>().As<ICore>();
            builder.RegisterType<RiskRepository>().As<IRiskRepository>();

            builder.RegisterInstance(Mapper.Engine).As<IMappingEngine>();

            var container = builder.Build();

            return container;
        }
    }
}