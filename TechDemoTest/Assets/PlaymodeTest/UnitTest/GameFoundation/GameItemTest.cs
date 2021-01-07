using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace Tech.Test
{
    public sealed class GameItemTest
    {
        private GameItem _gameItem;

        [SetUp]
        public void GameItemSetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));
            _gameItem = new GameItem();
        }

        [Test]
        [Performance]
        public void GameItemAddTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                /*_gameItem.StackableItemValueChanged()
                    .Subscribe(val => Debug.Log($"{val.definition.displayName} : {val.quantity}"));*/

                Measure.Method(() => _gameItem.IncreaseQuantity("stackable"))
                    .SampleGroup("Adding Stackable Item")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });

            // Use the Assert class to test conditions.
        }

        [Test]
        [Performance]
        public void GameItemRemoveTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                _gameItem.SetQuantity("stackable", 100000);

                /*_gameItem.StackableItemValueChanged()
                    .Subscribe(val => Debug.Log($"{val.definition.displayName} : {val.quantity}"));*/

                Measure.Method(() => _gameItem.DecreaseQuantity("stackable"))
                    .SampleGroup("Removing Stackable Item")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });
        }

        [Test]
        [Performance]
        public void GameItemZeroQuantityTestSimplePasses()
        {
            _gameItem.SetQuantity("stackable", 1000);

            // _gameItem.StackableItemValueChanged().Subscribe(val => {Debug.Log($"{val.definition.displayName} : {val.quantity}"); });

            using (Measure.Scope("Zero Quantity"))
            {
                _gameItem.ZeroQuantity("stackable");
            }

            using (Measure.Scope("Setting Quantity"))
            {
                _gameItem.SetQuantity("stackable", 1000);
            }
        }

        [TearDown]
        public void GameItemTearDown()
        {
            _gameItem.Dispose();
        }
    }
}