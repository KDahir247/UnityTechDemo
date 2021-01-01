using System.Collections;
using NUnit.Framework;
using UniRx;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public class EventSystemTest
    {
        [Test]
        [Performance]
        public void EventSystemTestSimplePasses()
        {
            Assert.DoesNotThrow(() =>
            {
                using (Measure.Scope("MessagePack Receive Query"))
                {
                    /*MessageBroker.Default.Receive<int>().Subscribe(value =>
                    {
                        if (value == 2)
                        {
                            Debug.Log(value);
                        }
                    });*/
                    //MessageBroker.Default.Receive<int>().Subscribe(value => Debug.Log(value));
                    MessageBroker.Default.Receive<int>()
                        .Where(v => v == 2)
                        .Subscribe(value => Debug.Log(value)); //fastest and low gc calls use where filter.
                }

                /*using (Measure.Scope("MessageBroker Receive WithOutQuery"))
                {
                    MessageBroker.Default.Receive<int>().Subscribe(value => Debug.Log(value));
                }*/

                // Use the Assert class to test conditions
                Measure.Method(() => MessageBroker.Default.Publish(Random.Range(0, 5)))
                    .SampleGroup("Publish event")
                    .WarmupCount(5)
                    .IterationsPerMeasurement(500)
                    .MeasurementCount(30)
                    .GC()
                    .Run();
            });
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator EventSystemTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}