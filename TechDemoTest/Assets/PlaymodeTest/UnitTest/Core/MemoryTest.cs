using System.Collections;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine.Profiling;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public sealed class MemoryTest
    {
        //1 mb is equal to 1048576 bytes so we want to measure in MB 

        [Test]
        [Performance]
        [Version("1")]
        public void MemoryTestTotalAllocatedMemorySimpleTestPasses()
        {
            // Use the Assert class to test conditions.
            var allocated = new SampleGroup("TotalAllocatedMemory", SampleUnit.Megabyte);
            Measure.Custom(allocated, Profiler.GetTotalAllocatedMemoryLong() / 1048576f);
        }

        [Test]
        [Performance]
        [Version("1")]
        public void MemoryTestReservedMemorySimpleTestPasses()
        {
            var reserved = new SampleGroup("TotalReservedMemory", SampleUnit.Megabyte);
            Measure.Custom(reserved, Profiler.GetTotalReservedMemoryLong() / 1048576f);
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator TestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}