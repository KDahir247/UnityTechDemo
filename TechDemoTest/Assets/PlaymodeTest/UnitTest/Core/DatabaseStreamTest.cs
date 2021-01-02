using System;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Tech.Test
{
    public sealed class DatabaseStreamTest
    {
        private DatabaseStream _stream;

        [SetUp]
        public void DatabaseStreamSetUp()
        {
            _stream = new DatabaseStream();
        }

        [Test]
        [Performance]
        public void DatabaseStreamTryLoadDatabaseTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.DoesNotThrow(() => Measure.Method(() => _stream.TryGetDatabase(FileDestination.TestDestination))
                .WarmupCount(5)
                .IterationsPerMeasurement(50)
                .MeasurementCount(20)
                .GC()
                .Run());

            Assert.Throws<NullReferenceException>(() => _stream.TryGetDatabase(FileDestination.None));
        }

        [Test]
        [Performance]
        public void DatabaseStreamGetDatabaseFileNameTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                Measure.Method(() => _stream.GetDatabaseFileName(FileDestination.TestDestination))
                    .WarmupCount(5)
                    .IterationsPerMeasurement(50)
                    .MeasurementCount(20)
                    .GC()
                    .Run();

                var testFileName = _stream.GetDatabaseFileName(FileDestination.TestDestination);
                StringAssert.IsMatch(testFileName, "test-data");
                testFileName = _stream.GetDatabaseFileName(FileDestination.None);
                StringAssert.IsMatch(testFileName, "");
            });
        }
    }
}