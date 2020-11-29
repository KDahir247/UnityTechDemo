using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using UnityEngine;
using ZLogger.Providers;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ZLogger.Providers
{
    [ProviderAlias("ZLoggerUnity")]
    public class ZLoggerUnityLoggerProvider : ILoggerProvider
    {
        private readonly UnityDebugLogProcessor debugLogProcessor;

        public ZLoggerUnityLoggerProvider(IOptions<ZLoggerOptions> options)
        {
            debugLogProcessor = new UnityDebugLogProcessor(options.Value);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new AsyncProcessZLogger(categoryName, debugLogProcessor);
        }

        public void Dispose()
        {
        }
    }

    public class UnityDebugLogProcessor : IAsyncLogProcessor
    {
        private readonly ZLoggerOptions options;

        public UnityDebugLogProcessor(ZLoggerOptions options)
        {
            this.options = options;
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public void Post(IZLoggerEntry log)
        {
            try
            {
                var msg = log.FormatToString(options, null);
                switch (log.LogInfo.LogLevel)
                {
                    case LogLevel.Trace:
                    case LogLevel.Debug:
                    case LogLevel.Information:
                        Debug.Log(msg);
                        break;
                    case LogLevel.Warning:
                    case LogLevel.Critical:
                        Debug.LogWarning(msg);
                        break;
                    case LogLevel.Error:
                        if (log.LogInfo.Exception != null)
                            Debug.LogException(log.LogInfo.Exception);
                        else
                            Debug.LogError(msg);
                        break;
                    case LogLevel.None:
                        break;
                }
            }
            finally
            {
                log.Return();
            }
        }
    }
}

namespace ZLogger
{
    public static class ZLoggerUnityExtensions
    {
        public static ILoggingBuilder AddZLoggerUnityDebug(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ZLoggerUnityLoggerProvider>(x =>
                    new ZLoggerUnityLoggerProvider(x.GetService<IOptions<ZLoggerOptions>>())));
            LoggerProviderOptions.RegisterProviderOptions<ZLoggerOptions, ZLoggerUnityLoggerProvider>(builder.Services);

            return builder;
        }

        public static ILoggingBuilder AddZLoggerUnityDebug(this ILoggingBuilder builder,
            Action<ZLoggerOptions> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            builder.AddZLoggerUnityDebug();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}