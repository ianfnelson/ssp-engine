using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using AutoMapper;

namespace SspEngineClient
{
    public class RiskRepository : IRiskRepository
    {
        private readonly IMappingEngine _mappingEngine;

        public RiskRepository(IMappingEngine mappingEngine)
        {
            _mappingEngine = mappingEngine;
        }

        public IEnumerable<SspEngine.DomainModel.Risk> GetRisks()
        {
            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var folderPath = Path.Combine(executingDirectory, "RiskFolder");

            var di = new DirectoryInfo(folderPath);

            var ser = new XmlSerializer(typeof (Risk));

            foreach (var file in di.EnumerateFiles())
            {
                Risk risk;
                using (XmlReader reader = XmlReader.Create(file.FullName))
                {
                    risk = (Risk) ser.Deserialize(reader);
                }

                if (string.IsNullOrWhiteSpace(risk.KeptPostcode)) risk.KeptPostcode = risk.Address.Postcode;

                var riskDomain = _mappingEngine.Map<SspEngine.DomainModel.Risk>(risk);

                yield return riskDomain;
            }
        }
    }
}