using System.Collections;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public sealed class UnitaskBootstrapTest
    {
        //TODO not yet required since ECS isn't incorporated
        [Test]
        public void UnitaskBootstrapTestSimplePasses()
        {
            // Use the Assert class to test conditions.
            Assert.IsTrue(PlayerLoopHelper.IsInjectedUniTaskPlayerLoop());
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator UnitaskBootstrapTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}