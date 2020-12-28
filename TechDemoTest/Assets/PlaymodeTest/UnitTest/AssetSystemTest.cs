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
    public class AssetSystemTest
    {
        private AssetSystem<GameObject> _assetSystem;
        private EntityInfo _entityInfo;
        readonly List<GameObject> entityContainerLoop = new List<GameObject>(1);

        [SetUp]
        public void AssetSystemSetUp()
        {
            _assetSystem = new AssetSystem<GameObject>();
            _entityInfo = new EntityInfo("Cube", new InstantiationParameters(Vector3.zero, Quaternion.identity, null));
        }

        [Test, Performance]
        public void AssetSystemCanceledTestSimplePasses()
        { 
            List<GameObject> entityContainer = new List<GameObject>(1);

            CancellationTokenSource cts = new CancellationTokenSource();
            
            cts.Cancel();
            
            Assert.DoesNotThrow( async () =>
            {
                using (Measure.Scope("Loading Asset Canceled"))
                {
                    _assetSystem.LoadAsset(_entityInfo, entityContainer, null, cts.Token).Forget();
                }
            });
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest, Performance]
        public IEnumerator AssetSystemTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            
            //Multiple Instantiation average of 10 instantiation call
            Assert.DoesNotThrow(() => 
                Measure
                .Method(() => _assetSystem.LoadAsset(_entityInfo, entityContainerLoop, null, CancellationToken.None).Forget())
                .SampleGroup("Loading Asset Iteration")
                .GC()
                .Run());

            yield return Measure.Frames()
                .MeasurementCount(20)
                .SampleGroup("Frame Performance Measurement")
                .Run();

            
            using (Measure.Scope("Unloading All Asset"))
            {
                _assetSystem.UnloadAllAsset(entityContainerLoop);
            }
        }
    }
}