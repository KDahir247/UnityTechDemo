using System;
using MasterData;
using MessagePack.Resolvers;
using Moq;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace Tech.Test
{
    public sealed class StaticDbBuilderTest
    {
        private StaticDbBuilder _notCorrectlySetUpBuilder;
        private StaticDbBuilder _staticDbBuilder;
        private Mock<IStream> _streamMock;

        [SetUp]
        public void StaticDbBuilderSetUp()
        {
            _streamMock = new Mock<IStream>();

            _streamMock.Setup(stream => stream.Builder)
                .Returns(new DatabaseBuilder(StaticCompositeResolver.Instance));

            _streamMock.Setup(stream => stream.GetDatabaseFileName(It.IsAny<FileDestination>()))
                .Returns("test-data");

            _staticDbBuilder = new StaticDbBuilder(_streamMock.Object);
            _notCorrectlySetUpBuilder = new StaticDbBuilder(null);
        }

        [Test]
        [Performance]
        public void StaticDbBuilderTestSimplePasses()
        {
            Assert.DoesNotThrow(async () =>
            {
                using (Measure.Scope())
                {
                    _staticDbBuilder.StaticallyMutateDatabase(FileDestination.TestDestination, builder =>
                    {
                        builder.Append(new TestTable[2]
                        {
                            new TestTable {Index = 0, Name = "TestingName"},
                            new TestTable {Index = 1, Name = "Created2ndName"}
                        });

                        return builder;
                    });

                    await _staticDbBuilder.BuildToDatabaseAsync();

                    var memoryDatabaseTest = _streamMock.Object.TryGetDatabase(FileDestination.TestDestination);

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
                _notCorrectlySetUpBuilder.StaticallyMutateDatabase(FileDestination.TestDestination,
                    builder => builder));
        }
    }
}