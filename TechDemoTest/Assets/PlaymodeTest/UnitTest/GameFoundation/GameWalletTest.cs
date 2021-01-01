using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public class GameWalletTest
    {
        private GameWallet gameWallet;

        [SetUp]
        public void GameWalletSetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));
            // gameObject.Object.gameObject.AddComponent<GameFoundationInit>();
        }

        [Test][Performance]
        public void GameWalletRemoveTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                using (gameWallet = new GameWallet("fakeMoney", "realMoney"))
                {
                    gameWallet.ResetWalletAmount("fakeMoney");
                    gameWallet.AddToWallet("fakeMoney", 20000);

                    Measure
                        .Method((() => gameWallet.RemoveFromWallet("fakeMoney", 1)))
                        .WarmupCount(3)
                        .IterationsPerMeasurement(30)
                        .SampleGroup("Testing Remove Wallet")
                        .MeasurementCount(9)
                        .GC()
                        .Run();
                }
            });
        }

        [Test]
        [Performance]
        public void GameWalletAddedTestSimplePasses()
        {

            Assert.DoesNotThrow(() =>
            {
                using (gameWallet = new GameWallet("fakeMoney", "realMoney"))
                {
                    gameWallet.ResetWalletAmount("fakeMoney");

                    Measure
                        .Method(() => gameWallet.AddToWallet("fakeMoney", 10000))
                        .WarmupCount(3)
                        .IterationsPerMeasurement(20)
                        .SampleGroup("Testing Add to Wallet")
                        .MeasurementCount(7)
                        .GC()
                        .Run();

                    gameWallet.Save();
                }
            });
        }
    }
}