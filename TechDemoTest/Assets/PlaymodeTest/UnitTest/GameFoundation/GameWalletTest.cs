using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public sealed class GameWalletTest
    {
        private GameWallet _gameWallet;

        [SetUp]
        public void GameWalletSetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));
            _gameWallet = new GameWallet();
        }

        [Test]
        [Performance]
        public void GameWalletRemoveTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                _gameWallet.ResetWalletAmount("fakeMoney");
                _gameWallet.AddToWallet("fakeMoney", 20000);

                Measure
                    .Method(() => _gameWallet.RemoveFromWallet("fakeMoney", 1))
                    .WarmupCount(3)
                    .IterationsPerMeasurement(30)
                    .SampleGroup("Testing Remove Wallet")
                    .MeasurementCount(9)
                    .GC()
                    .Run();
            });
        }

        [Test]
        [Performance]
        public void GameWalletAddedTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                _gameWallet.ResetWalletAmount("fakeMoney");

                Measure
                    .Method(() => _gameWallet.AddToWallet("fakeMoney", 10000))
                    .WarmupCount(3)
                    .IterationsPerMeasurement(20)
                    .SampleGroup("Testing Add to Wallet")
                    .MeasurementCount(7)
                    .GC()
                    .Run();

                _gameWallet.Save();
            });
        }

        [TearDown]
        public void GameWalletTearDown()
        {
            _gameWallet.Dispose();
        }
    }
}