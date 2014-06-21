using System;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public class VehicleKeptCheck : ICheck
    {
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
            throw new NotImplementedException();
        }
    }
}