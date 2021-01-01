using System;
using MasterData;
using MessagePack.Resolvers;
using Moq;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Tech.Test
{
    public class StaticDbBuilderTest
    {
        private StaticDbBuilder notCorrectlySetUpBuilder;
        private StaticDbBuilder staticDbBuilder;
        private Mock<IStream> streamMock;

        [SetUp]
        public void StaticDbBuilderSetUp()
        {
            streamMock = new Mock<IStream>();

            streamMock.Setup(stream => stream.Builder)
                .Returns(new DatabaseBuilder(StaticCompositeResolver.Instance));

            streamMock.Setup(stream => stream.GetDatabaseFileName(It.IsAny<FileDestination>()))
                .Returns("test-data");

            staticDbBuilder = new StaticDbBuilder(streamMock.Object);
            notCorrectlySetUpBuilder = new StaticDbBuilder(null);
        }

        [Test]
        [Performance]
        public void StaticDbBuilderTestSimplePasses()
        {
            Assert.DoesNotThrow(async () =>
            {
                using (Measure.Scope())
                {
                    staticDbBuilder.StaticallyMutateDatabase(FileDestination.TestDestination, builder =>
                    {
                        builder.Append(new TestTable[2]
                        {
                            new TestTable {Index = 0, Name = "TestingName"},
                            new TestTable {Index = 1, Name = "Created2ndName"}
                        });

                        return builder;
                    });

                    await staticDbBuilder.BuildToDatabaseAsync();

                    var memoryDatabaseTest = streamMock.Object.TryGetDatabase(FileDestination.TestDestination);

                    Assert.IsNotNull(memoryDatabaseTest);

                    Assert.That(memoryDatabaseTest.TestTableTable.All.Count, Is.EqualTo(2));

                    Assert.NotNull(memoryDatabaseTest.TestTableTable.FindByName("TestingName"));
                    Assert.NotNull(memoryDatabaseTest.TestTableTable.FindByIndex(0));

                    Assert.NotNull(memoryDatabaseTest.TestTableTable.FindByIndex(1));
                    Assert.NotNull(memoryDatabaseTest.TestTableTable.FindByName("Created2ndName"));
                }
            });
        }

        [Test]
        public void StaticDbBuilderExceptionHandleTestSimplePasses()
        {
            Assert.Throws<NullReferenceException>(() =>
                notCorrectlySetUpBuilder.StaticallyMutateDatabase(FileDestination.TestDestination,
                    builder => builder));
        }
    }
}