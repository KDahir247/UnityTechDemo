using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public sealed class GameStoreTest
    {
        private GameInventory _gameInventory;
        private GameStore _gameStore;
        private GameWallet _gameWallet;

        [SetUp]
        public void GameStoreTestSetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));
            _gameStore = new GameStore();
            _gameInventory = new GameInventory();
            _gameWallet = new GameWallet();
        }

        [Test]
        public void GameStoreNullTestSimplePasses()
        {
            Assert.DoesNotThrow(() => Assert.IsNull(_gameStore.RetrieveStoreTransactions("RandomStoreName")));
            // Use the Assert class to test conditions.
        }

        [Test]
        public void GameStoreNotNullTestSimplePasses()
        {
            Assert.DoesNotThrow(() => Assert.IsNotNull(_gameStore.RetrieveStoreTransactions("main")));
        }

        [Test]
        [Performance]
        public void GameStoreRetrieveAllTransactionsTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                Assert.NotNull(_gameStore.RetrieveStoreTransactions("main"));
                Assert.That(_gameStore.RetrieveStoreTransactions("main")?.Count, Is.GreaterThan(0));
                Measure.Method(() => _gameStore.RetrieveStoreTransactions("main"))
                    .SampleGroup("Retrieve Store Transaction")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });
        }

        [Test]
        [Performance]
        public void GameStorePurchaseTestSimplePasses()
        {
            //Uncomment for logging make gc call high due to debug.log call
            /*_gameWallet.ResetWalletAmount("fakeMoney");
            _gameWallet.AddToWallet("fakeMoney", 10000);
            _gameInventory.InventoryValueChanged()
                .Subscribe(inventoryInfo => Debug.Log(inventoryInfo.definition.displayName));
            _gameWallet.WalletValueChanged()
                .Subscribe(walletInfo => Debug.Log($"Wallet Amount: {walletInfo.quantity}"));*/

            Assert.DoesNotThrow(() => Measure.Method(() => _gameStore.PurchaseFromStore("main", "rockBlueprint"))
                .SampleGroup("Purchase From Store")
                .WarmupCount(5)
                .IterationsPerMeasurement(10)
                .MeasurementCount(9)
                .GC()
                .Run());
        }

        [TearDown]
        public void GameStoreTearDown()
        {
            _gameInventory.Dispose();
            _gameStore.Dispose();
            _gameWallet.Dispose();
        }
    }
}