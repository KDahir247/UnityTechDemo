using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public static class LogManager
{
    private static readonly ILoggerFactory LoggerFactory;

    static LogManager()
    {
        LoggerFactory = UnityLoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.SetMinimumLevel(LogLevel.Trace);

            //AttachLogToFile("",builder);
            AttachLogToUnity(builder);
        });

        LoggerFactory.CreateLogger("Global");

        Application.quitting += () => LoggerFactory.Dispose();
    }

    private static void AttachLogToFile(string fileName, ILoggingBuilder builder)
    {
        builder.AddZLoggerFile(fileName, options =>
        {
            //Extra Features here
        });
    }

    private static void AttachLogToUnity(ILoggingBuilder builder)
    {
        builder.AddZLoggerUnityDebug(options =>
        {
            //Extra Features here
        });
    }

    [CanBeNull]
    public static ILogger<T> GetLogger<T>()
        where T : class
    {
        return LoggerFactory?.CreateLogger<T>();
    }

    [CanBeNull]
    public static ILogger GetLogger(string categoryName)
    {
        return LoggerFactory?.CreateLogger(categoryName);
    }
}