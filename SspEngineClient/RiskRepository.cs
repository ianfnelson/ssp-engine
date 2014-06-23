using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;
using log4net;

namespace SspEngineClient
{
    public class RiskRepository : IRiskRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private readonly IMappingEngine _mappingEngine;

        public RiskRepository(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }

        public IEnumerable<SspEngine.DomainModel.Risk> GetRisks()
        {
            Log.Debug("RiskRepository.GetRisks() - entered");

            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var folderPath = Path.Combine(executingDirectory, "RiskFolder");

            Log.DebugFormat("Seeking risks from {0}", folderPath);

            var di = new DirectoryInfo(folderPath);

            var ser = new XmlSerializer(typeof (Risk));

            foreach (var file in di.EnumerateFiles())
            {
                Risk risk;
                using (XmlReader reader = XmlReader.Create(file.FullName))
                {
                    risk = (Risk) ser.Deserialize(reader);
                }

                if (string.IsNullOrWhiteSpace(risk.KeptPostcode))
                {
                    Log.DebugFormat("Risk {0} has empty KeptPostcode - defaulting to address postcode", risk.Name);
                    risk.KeptPostcode = risk.Address.Postcode;
                }

                var riskDomain = _mappingEngine.Map<SspEngine.DomainModel.Risk>(risk);

                yield return riskDomain;
            }

            Log.Debug("RiskRepository.GetRisks() - exited");
        }
    }
}