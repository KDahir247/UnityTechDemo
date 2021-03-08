using System;
using System.Reflection;
using Cysharp.Text;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

//Script Happens Separately from ECS System

namespace Tech.Core
{
#if UNITY_EDITOR
    internal static class LogManager
    {
        private static readonly ILoggerFactory LoggerFactory;

        static LogManager()
        {
            LoggerFactory = UnityLoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Trace);

                AttachLogToFile(builder);
                AttachLogToUnity(builder);
            });

            Logger = LoggerFactory.CreateLogger("Global");

            Application.quitting += () =>
            {
                Logger.ZLog(LogLevel.Information, "Closing Logger and Disposing");
                LoggerFactory.Dispose();
            };
        }

        public static ILogger Logger { get; }

        private static void AttachLogToUnity(ILoggingBuilder builder)
        {
            builder.AddZLoggerUnityDebug(options =>
            {
                var prefixFormat = ZString.PrepareUtf8<LogLevel, DateTime>("[{0}][{1}]");
                options.PrefixFormatter = (writer, info) =>
                    prefixFormat.FormatTo(ref writer, info.LogLevel, info.Timestamp.DateTime.ToLocalTime());

                var exceptionFormat =
                    ZString.PrepareUtf8<string, string, MethodBase, Exception, string>(
                        "[{0}][{1}][{2}][{3}] {4}");

                options.ExceptionFormatter = (writer, exception) => exceptionFormat.FormatTo(ref writer,
                    exception.Source, exception.Message, exception.TargetSite, exception.InnerException,
                    exception.StackTrace);

                options.EnableStructuredLogging = true;
            });
        }

        private static void AttachLogToFile(ILoggingBuilder builder)
        {
            var valueStringBuilder = new Utf8ValueStringBuilder(true);

            valueStringBuilder.AppendFormat(@"{0}\Assets\Log\EditorLog{1}.log", Environment.CurrentDirectory,
                DateTime.Today.ToFileTime());

            builder.AddZLoggerFile(valueStringBuilder.ToString(),
                options =>
                {
                    var prefixFormat =
                        ZString.PrepareUtf8<DateTime, LogLevel, DateTime, Exception>("[{0}]\n[{1}][{2}]{3}\n");
                    options.PrefixFormatter = (writer, info) => prefixFormat.FormatTo(ref writer, DateTime.Now,
                        info.LogLevel,
                        info.Timestamp.Date.ToLocalTime(), info.Exception);

                    var exceptionFormat =
                        ZString.PrepareUtf8<DateTime, string, string, MethodBase, Exception, string>(
                            " [{0}] \n\n[{1}][{2}][{3}][{4}] {5}");

                    options.ExceptionFormatter = (writer, exception) => exceptionFormat.FormatTo(ref writer,
                        DateTime.Now,
                        exception.Source, exception.Message, exception.TargetSite, exception.InnerException,
                        exception.StackTrace);

                    options.JsonSerializerOptions.WriteIndented = true;
                });
        }

        public static ILogger<T> GetLogger<T>() where T : class
        {
            return LoggerFactory?.CreateLogger<T>();
        }

        public static ILogger GetLogger(string category)
        {
            return LoggerFactory.CreateLogger(category);
        }
    }
#endif
}