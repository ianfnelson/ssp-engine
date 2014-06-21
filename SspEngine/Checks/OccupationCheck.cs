using System;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public class OccupationCheck : ICheck
    {
        public string Description
        {
            get { return "Occupation Check"; }
        }

        public int Ordinality
        {
            get { return 1; }
        }

        public RatingResult RunCheck(Risk risk)
        {
            throw new NotImplementedException();
        }
    }
}