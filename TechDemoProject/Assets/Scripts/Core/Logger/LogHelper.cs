using System;
using System.IO;
using Cysharp.Text;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

//Script Happens Separately from ECS System 

namespace Tech.Core
{
#if UNITY_EDITOR
    public static class LogHelper
    {
        private static readonly bool ExcludeUnityLog = true;

        private static readonly ILogger Logger = LogManager.GetLogger("UnityLog");
        public static event III<int, string, string> OnDelete; //all passed by in reference (readonly reference)

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            if (!ExcludeUnityLog)
                LoggerHook();

            ClearLogFile();

            Application.quitting += () =>
            {
                if (OnDelete != null)
                    foreach (var @delegate in OnDelete.GetInvocationList())
                        OnDelete -= @delegate as III<int, string, string>;

                OnDelete = null;

                Application.logMessageReceived -= ApplicationOnlogMessageReceived;
                Application.logMessageReceivedThreaded -= ApplicationOnlogMessageReceived;
                Application.lowMemory -= ApplicationOnlowMemory;
            };
        }

        private static void LoggerHook()
        {
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
            Application.logMessageReceivedThreaded += ApplicationOnlogMessageReceived;
            Application.lowMemory += ApplicationOnlowMemory;
        }

        private static void ApplicationOnlowMemory()
        {
            Logger.ZLogWarning("Low on Memory");
        }

        private static void ApplicationOnlogMessageReceived(string condition, string stacktrace, LogType type)
        {
            var valueStringBuilder = new Utf8ValueStringBuilder(true);

            valueStringBuilder.AppendFormat(@"[{0}] {1} {2}", type, condition, stacktrace);
            var builder = valueStringBuilder.ToString();

            switch (type)
            {
                case LogType.Log:
                    Logger.ZLogInformation(builder);
                    break;
                case LogType.Warning:
                    Logger.ZLogWarning(builder);
                    break;
                case LogType.Assert:
                    Logger.ZLogDebug(builder);
                    break;
                case LogType.Error:
                    Logger.ZLogError(builder);
                    break;
                case LogType.Exception:
                    Logger.ZLogCritical(builder);
                    break;
                default:
                    Logger.ZLogCritical("Switch has reach out of bounds on Unity Log");
                    break;
            }
        }

        private static void ClearLogFile()
        {
            var directory = Directory.GetFiles($@"{Environment.CurrentDirectory}\Assets\Log\");

            foreach (var file in directory)
            {
                var date = File.GetLastWriteTimeUtc(file);

                if (!(Mathf.Abs(date.Subtract(DateTime.Now).Days) > 10.0)) continue;

                OnDelete?.Invoke(date.Day, $@"{Environment.CurrentDirectory}\Assets\Log\", file);

                File.Delete(file);
            }
        }
    }
#endif
}