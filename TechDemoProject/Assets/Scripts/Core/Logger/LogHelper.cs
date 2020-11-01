using System;
using System.IO;
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
        public static bool ExcludeUnityLog = true;
        private static readonly ILogger Logger = LogManager.GetLogger("UnityLog");
        public static event VII<int, string, string> OnDelete;

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
                        OnDelete -= @delegate as VII<int, string, string>;

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
            //TODO use a utf8 StringBuilder
            var format = $"[{type}] {condition} {stacktrace}";
            switch (type)
            {
                case LogType.Log:
                    Logger.ZLogInformation(format);
                    break;
                case LogType.Warning:
                    Logger.ZLogWarning(format);
                    break;
                case LogType.Assert:
                    Logger.ZLogDebug(format);
                    break;
                case LogType.Error:
                    Logger.ZLogError(format);
                    break;
                case LogType.Exception:
                    Logger.ZLogCritical(format);
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

                if (Mathf.Abs(date.Date.ToLocalTime().Day - DateTime.Today.Day) > 10)
                {
                    OnDelete?.Invoke(date.Day, $@"{Environment.CurrentDirectory}\Assets\Log\", file);
                    File.Delete(file);
                }
            }
        }
    }
#endif
}