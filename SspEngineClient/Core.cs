using System;
using System.Reflection;
using log4net;
using SspEngine;

namespace SspEngineClient
{
    public class Core : ICore
    {
        private readonly IRiskRepository _riskRepository;
        private readonly IEngine _engine;

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Core(IRiskRepository riskRepository, IEngine engine)
        {
            _riskRepository = riskRepository;
            _engine = engine;
        }

        public void RateRisks()
        {
            var risks = _riskRepository.GetRisks();

            foreach (var risk in risks)
            {
                var result = _engine.RunChecks(risk);

                Log.InfoFormat("{0} - {1}", risk.Name, result);
            }
        }
    }
}