using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.Core
{
    public static class UnitaskBootstrap
    {
        private static readonly ILogger Logger =
            LogManager.GetLogger("UnitaskBootStrapLogger");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeUnitask()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref loop);

            if (false) return; //Hardcoded verbose

            Logger.ZLogInformation("ReInitialized Unitask after Initialization of ECS is complete");

            var stringBuilder = ZString.CreateUtf8StringBuilder();
            stringBuilder.AppendFormat("Unitask Player loop is ready? {0}",
                PlayerLoopHelper.IsInjectedUniTaskPlayerLoop());

            Logger.ZLogInformation(stringBuilder.ToString());

            if (PlayerLoopHelper.IsInjectedUniTaskPlayerLoop())
                Logger.ZLogInformation("Initialized Unitask correctly for ECS");
            else
                Logger.ZLogError("Initialization of Unitask has failed for ECS");

            PlayerLoopHelper.DumpCurrentPlayerLoop();
            stringBuilder.Dispose();
        }
    }
}