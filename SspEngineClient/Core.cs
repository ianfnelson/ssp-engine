using System;
using SspEngine;

namespace SspEngineClient
{
    public class Core : ICore
    {
        private readonly IRiskRepository _riskRepository;
        private readonly IEngine _engine;

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

                Console.WriteLine("{0} - {1}", risk.Name, result);
            }
        }
    }
}