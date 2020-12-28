using System.Collections;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public class SceneSystemTest
    {
        private SceneSystem _sceneSystem;
        
        [SetUp]
        public void SceneSystemSetUp()
        {
            _sceneSystem = new SceneSystem();
        }
        
        [Test,Performance]
        public void SceneSystemTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.DoesNotThrow((() =>
            {
                _sceneSystem.LoadScene();
                _sceneSystem.UnloadScene();
            }));
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest, Performance]
        public IEnumerator SceneSystemTestWithEnumeratorPasses()
        {
            
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}