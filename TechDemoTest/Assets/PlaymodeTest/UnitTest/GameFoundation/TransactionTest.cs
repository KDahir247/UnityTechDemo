using NUnit.Framework;
using UniRx;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public sealed class TransactionTest
    {
        private GameInventory _gameInventory;
        private GameTransaction _gameTransaction;
        private GameWallet _wallet;

        [SetUp]
        public void TransactionTestSetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));

            _wallet = new GameWallet();
            _gameInventory = new GameInventory();
            _gameTransaction = new GameTransaction();
        }

        [Test]
        [Performance]
        public void TransactionFullTestSimplePasses()
        {
            // Use the Assert class to test conditions.

            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope("Full Game Foundation Integration"))
                {
                    _wallet.AddToWallet("fakeMoney", 1000);
                    _wallet.WalletValueChanged().Subscribe(val =>
                        Debug.Log($"Starting transaction. Cash amount is now {val.quantity}"));

                    _gameInventory.InventoryValueChanged()
                        .Subscribe(val => Debug.Log($"bought a {val.definition.displayName}"));

                    _gameTransaction.OnTransactionCompleted().Subscribe(_ => Debug.Log("transaction Completed"));
                    _gameTransaction.OnTransactionFailed()
                        .Subscribe(error => Debug.Log($"{error?.InnerException?.Message}"));
                    _gameTransaction.Purchase("rockBlueprint");
                    _gameTransaction.Purchase("rockBlueprint");
                    _gameTransaction.Purchase("rockBlueprint");
                    _gameTransaction.Purchase("rockBlueprint");
                }
            });
        }

        [Test]
        [Performance]
        public void TransactionSuccessTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                _wallet.AddToWallet("fakeMoney", 200000);

                Measure.Method(() => _gameTransaction.Purchase("rockBlueprint"))
                    .SampleGroup("Transaction Success Purchase")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });
        }

        [Test]
        [Performance]
        public void TransactionFailedTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                _wallet.ResetWalletAmount("fakeMoney");

                Measure.Method(() => _gameTransaction.Purchase("rockBlueprint"))
                    .SampleGroup("Transaction Failed Purchase")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });
        }

        [TearDown]
        public void TransactionTestTearDown()
        {
            _gameInventory.Dispose();
            _wallet.Dispose();
            _gameTransaction.Dispose();
        }
    }
}