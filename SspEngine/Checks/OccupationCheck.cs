using System;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public class OccupationCheck : LookupCheck<string>, ICheck
    {
        public OccupationCheck() : base(new OccupationDataSource())
        {
        }

        public override RatingResult ResultIfDataMissing
        {
            get { return RatingResult.Decline; }
        }

        public override Func<Risk, string> KeyFunction
        {
            get { return x => x.Occupation; }
        }

        public string Description
        {
            get { return "Occupation Check"; }
        }

        public int Ordinality
        {
            get { return 1; }
        }
    }
}