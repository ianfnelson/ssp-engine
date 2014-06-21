using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using SspEngine.DomainModel;
using System.Linq;

namespace SspEngine.Checks
{
    public class OccupationDataSource : ILookupDataSource<string>
    {
        private static Dictionary<string, RatingResult> _lookupData; 

        public IDictionary<string, RatingResult> GetLookupData()
        {
            return _lookupData ?? (_lookupData = GetLookupDataFromFile());
        }

        private Dictionary<string, RatingResult> GetLookupDataFromFile()
        {
            var executingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(executingDirectory, "LookupData\\Occupations.xml");

            var ser = new XmlSerializer(typeof(Occupations));
            Occupations occupations;

            using (XmlReader reader = XmlReader.Create(filePath))
            {
                occupations = (Occupations) ser.Deserialize(reader);
            }

            return occupations.Occupation.ToDictionary(o => o.Desc, o => (RatingResult)Enum.Parse(typeof (RatingResult), o.Action), StringComparer.OrdinalIgnoreCase);
        }
    }
}