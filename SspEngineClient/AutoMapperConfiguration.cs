using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace SspEngineClient
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x => GetConfiguration(Mapper.Configuration));
        }

        private static void GetConfiguration(IConfiguration configuration)
        {
            var profiles = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof (Profile).IsAssignableFrom(x));

            foreach (var profile in profiles)
            {
                configuration.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        }
    }
}