using System.Collections;
using NUnit.Framework;
using UniRx;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public sealed class RewardTest
    {
        private GameInventory _gameInventory;
        private GameReward _gameReward;
        private GameWallet _gameWallet;

        [SetUp]
        public void RewardTestSetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));

            _gameReward = new GameReward();
            _gameWallet = new GameWallet();
            _gameInventory = new GameInventory();
        }

        [Test]
        [Performance]
        public void RewardTestWithListenerSimplePasses()
        {
            // Use the Assert class to test conditions.

            Assert.DoesNotThrow(() =>
            {
                _gameWallet.WalletValueChanged()
                    .Subscribe(val => Debug.Log($"Added {val.quantity} to the wallet amount"));
                _gameInventory.InventoryValueChanged()
                    .Subscribe(equip => Debug.Log($"added {equip.definition.displayName}"));

                _gameReward.OnClaimingRewardCompleted().Subscribe(_ => Debug.Log("Claimed reward"));
                _gameReward.OnClaimingRewardFailed().Subscribe(_ => Debug.Log("Failed to claim reward"));

                Measure.Method(() => _gameReward.Claim("dailyRockReward"))
                    .SampleGroup("Claim Reward")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });
        }

        [Test]
        [Performance]
        public void RewardTestRecursiveSimplePasses()
        {
            Assert.DoesNotThrow(() => Measure.Method(() => _gameReward.Claim("alwayReward"))
                .SampleGroup("Claim recursive Reward")
                .WarmupCount(5)
                .IterationsPerMeasurement(20)
                .MeasurementCount(10)
                .GC()
                .Run());
        }


        [Test]
        [Performance]
        public void RewardTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope("Single Claim Reward"))
                {
                    _gameReward.Claim("alwayReward");
                }
            });
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator RewardTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return new WaitForSeconds(5);

            _gameReward.Claim("dailyRockReward");

            yield return new WaitForSeconds(5);

            _gameReward.Claim("dailyRockReward");
        }

        [TearDown]
        public void RewardTearDown()
        {
            _gameInventory.Dispose();
            _gameWallet.Dispose();
        }
    }
}