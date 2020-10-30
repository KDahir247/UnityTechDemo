﻿using Cysharp.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;
using ZLogger;


namespace Tech.Core
{
    public static class UnitaskBootstrap
    {
        private static readonly Microsoft.Extensions.Logging.ILogger Logger =
            LogManager.GetLogger("UnitaskBootStrapLogger");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            PlayerLoopHelper.Initialize(ref loop);

            Logger.ZLogInformation("ReInitialized Unitask after Initialization of ECS is complete");


            Utf8ValueStringBuilder stringBuilder = ZString.CreateUtf8StringBuilder();
            stringBuilder.AppendFormat("Unitask Player loop is ready? {0}",
                PlayerLoopHelper.IsInjectedUniTaskPlayerLoop());

            Logger.ZLogInformation(stringBuilder.ToString());

            if (PlayerLoopHelper.IsInjectedUniTaskPlayerLoop())
            {
                Logger.ZLogInformation("Initialized Unitask correctly for ECS");
            }
            else
            {
                Logger.ZLogError("Initialization of Unitask has failed for ECS");
            }

            PlayerLoopHelper.DumpCurrentPlayerLoop();
            stringBuilder.Dispose();
        }
    }
}