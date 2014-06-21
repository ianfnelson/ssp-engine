using System;
using System.Configuration;

namespace SspEngine
{
    public abstract class AppSettingsProvider
    {
        private static AppSettingsProvider current;

        static AppSettingsProvider()
        {
            current = new DefaultAppSettingsProvider();
        }

        public static AppSettingsProvider Current
        {
            get { return current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                current = value;
            }
        }

        public abstract int VehicleKeptCheck_AcceptBelowMetres { get; }
        public abstract int VehicleKeptCheck_ReferBelowMetres { get; }

        public static void ResetToDefault()
        {
            current = new DefaultAppSettingsProvider();
        }
    }

    public class DefaultAppSettingsProvider : AppSettingsProvider
    {
        public override int VehicleKeptCheck_AcceptBelowMetres
        {
            get { return int.Parse(ConfigurationManager.AppSettings["VehicleKeptCheck_AcceptBelowMetres"]); }
        }

        public override int VehicleKeptCheck_ReferBelowMetres
        {
            get { return int.Parse(ConfigurationManager.AppSettings["VehicleKeptCheck_ReferBelowMetres"]); }
        }
    }
}