using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Unity.PerformanceTesting;
using ZLogger;

namespace Tech.Test
{
    public sealed class LogManagerTest
    {
        private static readonly ILogger<LogManagerTest> Logger = LogManager.GetLogger<LogManagerTest>();

        [Test]
        [Performance]
        public void LogManagerGenericLogTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope())
                {
                    Logger.ZLog(LogLevel.Trace, "Running Generic Trace ZLog Test");
                }
            });
        }

        [Test]
        [Performance]
        public void LogManagerWithoutPayLoadTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope())
                {
                    Logger.ZLogCritical("Running Critical ZLog Test ");
                    Logger.ZLogWarning("Running Warning ZLog Test");
                    Logger.ZLogDebug("Running Debug ZLog Test");
                    Logger.ZLogInformation("Running Information ZLog Test");
                    Logger.ZLogTrace("Running Trace ZLog Test");
                }
            });
        }

        [Test]
        [Performance]
        public void LogManagerWithoutPayLoadPassedArgsTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope())
                {
                    Logger.ZLog(LogLevel.Information, "Displaying 4 arguments {0},\t{1},\t{2},\t{3}", nameof(GetType),
                        nameof(Logger.GetType), this, Logger);
                }
            });
        }

        [Test]
        [Performance]
        public void LogManagerGCCallTestSimplePasses()
        {
            Assert.DoesNotThrow(() => Measure
                .Method(() => Logger.Log(LogLevel.Trace, "Going to log a lot to get the GC call"))
                .WarmupCount(5)
                .IterationsPerMeasurement(20)
                .MeasurementCount(9)
                .GC()
                .Run());
        }


        /*[Test]
        [Performance]
        public void LogManagerWithPayLoadTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                //Not yet implemented
            });
        }*/


        /*[Test]
        [Performance]
        public void LoadManagerWithFileSupportTestSimplePasses()
        {
            Assert.DoesNotThrow((() =>
            {
                //Not yet implemented
            }));
        }*/
    }
}