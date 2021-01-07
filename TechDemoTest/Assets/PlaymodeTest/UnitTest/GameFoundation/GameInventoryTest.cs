using System;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tech.Test
{
    public sealed class GameInventoryTest
    {
        private GameInventory _gameInventory;
        private GameInventory _innerGameInventory;

        [SetUp]
        public void GameInventorySetUp()
        {
            Object.Instantiate(Resources.Load<GameObject>("Prefabs/GameFoundation"));
            _gameInventory = new GameInventory();
            _innerGameInventory = new GameInventory();
        }

        [Test]
        [Performance]
        public void GameInventoryAddTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                //_innerGameInventory.InventoryValueChanged().Subscribe(_ => Debug.Log("Item has been added"));

                Measure.Method(() => _gameInventory.AddToInventory("rock"))
                    .SampleGroup("Adding Inventory")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(50)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
                //Logic
            });
            // Use the Assert class to test conditions.
        }

        [Test]
        [Performance]
        public void GameInventoryRemove1TestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                /*
                _gameInventory
                    .InventoryValueChanged()
                    .Subscribe(itemPair =>
                        Debug.Log(
                            $"Removed {itemPair.Item1.definition.displayName}. there is a total of {itemPair.Item2.Count} rocks left"));
                            */

                Measure.Method(() => _gameInventory.RemoveInventory("rock"))
                    .SampleGroup("Removing Inventory")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .GC()
                    .Run();
            });
        }

        [Test]
        [Performance]
        public void GameInventoryRemove2AllTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope("Removing all inventory"))
                {
                    /*_gameInventory.InventoryValueChanged()
                        .Subscribe(itemPair => Debug.Log(itemPair.Item2.Count));*/
                    _gameInventory.RemoveAllFromInventoryType("rock");
                }
            });
        }

        [Test]
        public void GameInventoryAddIncorrectKeyTestSimplePasses()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                _gameInventory.AddToInventory("foo");
                _gameInventory.RemoveInventory("bar");
                _gameInventory.RemoveAllFromInventoryType("fizz");
            });
        }

        [TearDown]
        public void GameInventoryTearDown()
        {
            _gameInventory.Dispose();
            _innerGameInventory.Dispose();
        }
    }
}