using System;
using System.Collections.Generic;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public abstract class LookupCheck<TKey> 
    {
        protected LookupCheck(ILookupDataSource<TKey> dataSource)
        {
            LookupData = dataSource.GetLookupData();
        }

        public abstract RatingResult ResultIfDataMissing { get; }

        public abstract Func<Risk, TKey> KeyFunction { get; } 

        private IDictionary<TKey, RatingResult> LookupData { get; set; }

        public virtual RatingResult RunCheck(Risk risk)
        {
            var key = KeyFunction(risk);

            RatingResult result;
            return LookupData.TryGetValue(key, out result) ? result : ResultIfDataMissing;
        }
    }
}