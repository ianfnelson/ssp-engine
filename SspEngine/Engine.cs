using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using SspEngine.DomainModel;

namespace SspEngine
{
    public interface IEngine
    {
        RatingResult RunChecks(Risk risk);
    }

    public class Engine : IEngine
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Engine(IEnumerable<ICheck> checks)
        {
            Checks = checks.OrderBy(c => c.Ordinality).ToList();
        }

        public IList<ICheck> Checks { get; private set; }

        public RatingResult RunChecks(Risk risk)
        {
            Log.DebugFormat("Engine.RunChecks() - entered");

            Log.DebugFormat("Running checks for risk {0}", risk.Name);

            var result = RatingResult.Accept;

            foreach (var check in Checks)
            {
                Log.DebugFormat("Running check {0} for risk {1}", check.Description, risk.Name);

                var checkResult = check.RunCheck(risk);

                Log.DebugFormat("Check {0} for risk {1} returned {2}", check.Description, risk.Name, checkResult);

                // Note the worst result so far
                if (checkResult > result)
                {
                    result = checkResult;
                }

                // If we get a Decline from any individual check, we can skip the following checks.
                if (checkResult == RatingResult.Decline)
                {
                    break;
                }
            }

            Log.DebugFormat("Combined checks for risk {0} returned {1}", risk.Name, result);

            Log.DebugFormat("Engine.RunChecks() - exited");

            return result;
        }
    }
}