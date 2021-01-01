using System;
using MasterData;
using MessagePack.Resolvers;
using Moq;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public class DynamicDbBuilderTest
    {
        private DynamicDbBuilder dynamicBuilder;
        private DynamicDbBuilder notCorrectlySetUpBuilder;
        private Mock<IStream> streamMock;

        [SetUp]
        public void DynamicDbBuilderSetUp()
        {
            streamMock = new Mock<IStream>();

            streamMock.Setup(stream => stream.Builder)
                .Returns(new DatabaseBuilder(StaticCompositeResolver.Instance));

            streamMock.Setup(stream => stream.TryGetDatabase(It.IsAny<FileDestination>()))
                .Returns(new MemoryDatabase(Resources.Load<TextAsset>("test-data").bytes));

            streamMock.Setup(stream => stream.GetDatabaseFileName(It.IsAny<FileDestination>()))
                .Returns("test-data");

            dynamicBuilder = new DynamicDbBuilder(streamMock.Object);
            notCorrectlySetUpBuilder = new DynamicDbBuilder(null);
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
                    dynamicBuilder.DynamicallyMutateDatabase(FileDestination.TestDestination, builder =>
                    {
                        builder.Diff(new TestTable[1]
                        {
                            new TestTable {Index = 5, Name = "AddedName"}
                        });
                        return builder;
                    });

                    await dynamicBuilder.BuildToDatabaseAsync();
                }
            });
        }

        [Test]
        public void DynamicDbBuilderQueryTestSimplePasses()
        {
            var memoryDatabase = streamMock.Object.TryGetDatabase(FileDestination.TestDestination);

            Assert.IsNotNull(memoryDatabase);
            Assert.AreEqual(memoryDatabase.TestTableTable.All.Count, 3);

            Assert.NotNull(memoryDatabase.TestTableTable.FindByIndex(5));
            Assert.NotNull(memoryDatabase.TestTableTable.FindByName("AddedName"));
        }

        [Test]
        public void DynamicDbExceptionHandleTestSimplePasses()
        {
            Assert.Throws<NullReferenceException>(() =>
                notCorrectlySetUpBuilder.DynamicallyMutateDatabase(FileDestination.TestDestination,
                    immutableBuilder => immutableBuilder));
        }
    }
}