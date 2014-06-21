using System;
using System.Collections.Generic;
using System.Linq;
using SspEngine.DomainModel;

namespace SspEngine
{
    public interface IEngine
    {
        RatingResult RunChecks(Risk risk);
    }

    public class Engine : IEngine
    {
        public IList<ICheck> Checks { get; private set; } 

        public Engine(params ICheck[] checks)
        {
            Checks = checks.OrderBy(c => c.Ordinality).ToList();
        }

        public RatingResult RunChecks(Risk risk)
        {
            var result = RatingResult.Accept;

            foreach (var check in Checks)
            {
                var checkResult = check.RunCheck(risk);

                // If we get a Decline from any individual check, we can skip the following checks.
                if (checkResult == RatingResult.Decline) return RatingResult.Decline;

                // Otherwise, we need to note the worst result so far.
                if (checkResult > result)
                {
                    result = checkResult;
                }
            }

            return result;
        }
    }
}