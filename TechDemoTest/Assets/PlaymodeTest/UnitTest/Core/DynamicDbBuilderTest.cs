using System;
using MasterData;
using MessagePack.Resolvers;
using Moq;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public sealed class DynamicDbBuilderTest
    {
        private DynamicDbBuilder _dynamicBuilder;
        private DynamicDbBuilder _notCorrectlySetUpBuilder;
        private Mock<IStream> _streamMock;

        [SetUp]
        public void DynamicDbBuilderSetUp()
        {
            _streamMock = new Mock<IStream>();

            _streamMock.Setup(stream => stream.Builder)
                .Returns(new DatabaseBuilder(StaticCompositeResolver.Instance));

            _streamMock.Setup(stream => stream.TryGetDatabase(It.IsAny<FileDestination>()))
                .Returns(new MemoryDatabase(Resources.Load<TextAsset>("test-data").bytes));

            _streamMock.Setup(stream => stream.GetDatabaseFileName(It.IsAny<FileDestination>()))
                .Returns("test-data");

            _dynamicBuilder = new DynamicDbBuilder(_streamMock.Object);
            _notCorrectlySetUpBuilder = new DynamicDbBuilder(null);
        }

        [Test]
        [Performance]
        public void DynamicDbBuilderTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.DoesNotThrow(async () =>
            {
                using (Measure.Scope())
                {
                    _dynamicBuilder.DynamicallyMutateDatabase(FileDestination.TestDestination, builder =>
                    {
                        builder.Diff(new[]
                        {
                            new TestTable {Index = 5, Name = "AddedName"}
                        });
                        return builder;
                    });

                    await _dynamicBuilder.BuildToDatabaseAsync();
                }
            });
        }

        [Test]
        public void DynamicDbBuilderQueryTestSimplePasses()
        {
            var memoryDatabase = _streamMock.Object.TryGetDatabase(FileDestination.TestDestination);

            Assert.IsNotNull(memoryDatabase);
            Assert.AreEqual(memoryDatabase.TestTableTable.All.Count, 3);

            Assert.NotNull(memoryDatabase.TestTableTable.FindByIndex(5));
            Assert.NotNull(memoryDatabase.TestTableTable.FindByName("AddedName"));
        }

        [Test]
        public void DynamicDbExceptionHandleTestSimplePasses()
        {
            Assert.Throws<NullReferenceException>(() =>
                _notCorrectlySetUpBuilder.DynamicallyMutateDatabase(FileDestination.TestDestination,
                    immutableBuilder => immutableBuilder));
        }
    }
}