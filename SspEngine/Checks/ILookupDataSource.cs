using System.Collections.Generic;
using SspEngine.DomainModel;

namespace SspEngine.Checks
{
    public interface ILookupDataSource<TKey>
    {
        IDictionary<TKey, RatingResult> GetLookupData();
    }
}