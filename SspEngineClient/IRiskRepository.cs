using System.Collections.Generic;

namespace SspEngineClient
{
    public interface IRiskRepository
    {
        IEnumerable<SspEngine.DomainModel.Risk> GetRisks();
    }
}