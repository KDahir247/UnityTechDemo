using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine.SceneManagement;

namespace Tech.Test
{
    public sealed class SceneSystemTest
    {
        private SceneSystem _sceneCanceledSystem;
        private SceneSystem _sceneSystem;
        private CancellationTokenSource cts;

        [SetUp]
        public void SceneSystemSetUp()
        {
            _sceneSystem = new SceneSystem(CancellationToken.None);
            cts = new CancellationTokenSource();
            cts.Cancel();
            _sceneCanceledSystem = new SceneSystem(cts.Token);
        }

        [Test]
        [Performance]
        public void SceneSystemSingleCanceledTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
                {
                    using (Measure.Scope())
                    {
                        _sceneCanceledSystem.LoadSceneAsync("NextScene", LoadSceneMode.Single).Forget();
                    }
                }
            );
        }

        [Test]
        [Performance]
        public void SceneSystemSingleTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.DoesNotThrow(async () =>
            {
                using (Measure.Scope("Await And Forget SceneLoading"))
                {
                    await _sceneSystem.LoadSceneAsync("NextScene", LoadSceneMode.Single);
                    _sceneSystem.LoadSceneAsync("NextScene", LoadSceneMode.Single).Forget();
                }
            });
        }

        [Test]
        [Performance]
        public void SceneSystemAdditiveTestSimplePasses()
        {
            Assert.DoesNotThrow(async () =>
            {
                using (Measure.Scope("Additive Scene Load & Unload"))
                {
                    var sceneInstance = await _sceneSystem.LoadSceneAsync("NextScene", LoadSceneMode.Additive);
                    _sceneSystem.UnloadScene(sceneInstance).Forget();
                }
            });
        }
    }
}