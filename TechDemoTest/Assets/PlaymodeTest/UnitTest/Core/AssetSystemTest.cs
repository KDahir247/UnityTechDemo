using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public sealed class AssetSystemTest
    {
        private readonly List<GameObject> entityContainerLoop = new List<GameObject>(1);
        private AssetSystem<GameObject> _assetSystem;
        private AssetSystem<GameObject> _assetSystemCanceled;


        private AssetInfo assetInfo;

        [SetUp]
        public void AssetSystemSetUp()
        {
            _assetSystem = new AssetSystem<GameObject>(CancellationToken.None);
            assetInfo = new AssetInfo("Cube", new InstantiationParameters(Vector3.zero, Quaternion.identity, null));

            var cts = new CancellationTokenSource();

            cts.Cancel();

            _assetSystemCanceled = new AssetSystem<GameObject>(cts.Token);
        }

        [Test]
        [Performance]
        public void AssetSystemCanceledTestSimplePasses()
        {
            var entityContainer = new List<GameObject>(1);

            Assert.Throws<OperationCanceledException>(() =>
            {
                using (Measure.Scope("Loading Asset Canceled"))
                {
                    _assetSystemCanceled.LoadAsset(assetInfo, entityContainer);
                }
            });
        }


        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        [Performance]
        public IEnumerator AssetSystemTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame

            //Multiple Instantiation average of 10 instantiation call
            Assert.DoesNotThrow(() =>
                Measure
                    .Method(() => _assetSystem.LoadAsset(assetInfo, entityContainerLoop))
                    .WarmupCount(5)
                    .IterationsPerMeasurement(20)
                    .MeasurementCount(10)
                    .SampleGroup("Loading Asset Iteration")
                    .GC()
                    .CleanUp(() =>
                    {
                        using (Measure.Scope("Unloading All Asset"))
                        {
                            _assetSystem.UnloadAllAsset(entityContainerLoop);
                        }
                    })
                    .Run());

            yield return Measure.Frames()
                .WarmupCount(10)
                .MeasurementCount(20)
                .SampleGroup("Frame Performance Measurement")
                .Run();
        }
    }
}