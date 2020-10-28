using System;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniRx;
using UnityEngine;
using ZLogger;

//Script Happens Separately from ECS System 

namespace Tech.Core
{
#if UNITY_EDITOR
    public static class LogManager
    {
        private static readonly Microsoft.Extensions.Logging.ILogger GlobalLogger;
        private static readonly ILoggerFactory LoggerFactory;
        //private static readonly CompositeDisposable Disposable = new CompositeDisposable();

        static LogManager()
        {
            LoggerFactory = UnityLoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                
                builder.SetMinimumLevel(LogLevel.Trace);

               //TODO Create Log Directory automatically depending on the day and store the correct log file in the correct folder
                Utf8ValueStringBuilder valueStringBuilder = new Utf8ValueStringBuilder(true);
                valueStringBuilder.AppendFormat(@"{0}\Assets\Log\EditorLog{1}.log",Environment.CurrentDirectory, DateTime.Today.ToFileTime() );
                builder.AddZLoggerFile(valueStringBuilder.ToString(),
                    options =>
                    {
                        Utf8PreparedFormat<DateTime, LogLevel, DateTime,Exception> prefixFormat =
                            ZString.PrepareUtf8<DateTime, LogLevel, DateTime, Exception>("[{0}]\n[{1}][{2}]{3}\n");
                        options.PrefixFormatter = (writer, info) => prefixFormat.FormatTo(ref writer, DateTime.Now,
                            info.LogLevel,
                            info.Timestamp.Date.ToLocalTime(), info.Exception);
                        
                        Utf8PreparedFormat<DateTime, string,string,MethodBase, Exception, string> exceptionFormat =
                            ZString.PrepareUtf8<DateTime, string, string, MethodBase, Exception, string>(
                                " [{0}] \n\n[{1}][{2}][{3}][{4}] {5}");
                        
                        options.ExceptionFormatter = (writer, exception) => exceptionFormat.FormatTo(ref writer,
                            DateTime.Now,
                            exception.Source, exception.Message, exception.TargetSite, exception.InnerException,
                            exception.StackTrace);

                        // options.StructuredLoggingFormatter = (writer, info) => { };
                        // options.EnableStructuredLogging = true;

                        options.JsonSerializerOptions.WriteIndented = true;
                    });
                
                builder.AddZLoggerUnityDebug(options => 
                {
                    Utf8PreparedFormat<LogLevel, DateTime> prefixFormat = ZString.PrepareUtf8<LogLevel, DateTime>("[{0}][{1}]");
                    options.PrefixFormatter = (writer, info) => prefixFormat.FormatTo(ref writer, info.LogLevel, info.Timestamp.DateTime.ToLocalTime());
                    
                    Utf8PreparedFormat<string,string,MethodBase,Exception,string> exceptionFormat =
                        ZString.PrepareUtf8<string, string, MethodBase, Exception, string>(
                            "[{0}][{1}][{2}][{3}] {4}");
                    
                    options.ExceptionFormatter = (writer, exception) => exceptionFormat.FormatTo(ref writer,
                        exception.Source, exception.Message, exception.TargetSite, exception.InnerException,
                        exception.StackTrace); 
                    
                    options.EnableStructuredLogging = true; 
                });
            });

            GlobalLogger = LoggerFactory.CreateLogger("Global");

            Application.quitting += () =>
            {
                Logger.ZLog(LogLevel.Information, "Closing Logger and Disposing");
                LoggerFactory.Dispose();
            };
        }
        
        public static Microsoft.Extensions.Logging.ILogger Logger => GlobalLogger;
        public static ILogger<T> GetLogger<T>() where T : class => LoggerFactory.CreateLogger<T>();

        public static Microsoft.Extensions.Logging.ILogger GetLogger(string category) =>
            LoggerFactory.CreateLogger(category);
    }
#endif

}