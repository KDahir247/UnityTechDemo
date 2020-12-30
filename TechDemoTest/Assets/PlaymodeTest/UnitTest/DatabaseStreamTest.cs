using System;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Tech.Test
{
    public sealed class DatabaseStreamTest
    {
        private DatabaseStream stream;

        [SetUp]
        public void DatabaseStreamSetUp()
        {
            stream = new DatabaseStream();
        }

        [Test]
        [Performance]
        public void DatabaseStreamTryLoadDatabaseTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.DoesNotThrow(() => Measure.Method(() => stream.TryGetDatabase(FileDestination.TestDestination))
                .WarmupCount(5)
                .IterationsPerMeasurement(50)
                .MeasurementCount(20)
                .GC()
                .Run());

            Assert.Throws<NullReferenceException>(() => stream.TryGetDatabase(FileDestination.None));
        }

        [Test]
        [Performance]
        public void DatabaseStreamGetDatabaseFileNameTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                Measure.Method(() => stream.GetDatabaseFileName(FileDestination.TestDestination))
                    .WarmupCount(5)
                    .IterationsPerMeasurement(50)
                    .MeasurementCount(20)
                    .GC()
                    .Run();

                var testFileName = stream.GetDatabaseFileName(FileDestination.TestDestination);
                StringAssert.IsMatch(testFileName, "test-data");
                testFileName = stream.GetDatabaseFileName(FileDestination.None);
                StringAssert.IsMatch(testFileName, "");
            });
        }
    }
}