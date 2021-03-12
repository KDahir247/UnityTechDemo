using System.Collections;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Tests;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tech.Test.ECS
{
    public sealed class UnitQueueEcsTest : ECSTestsFixture
    {
        private Entity _e;
        //private const int Offset = 2;
        [SetUp]
        public void UnitQueueEcsTestSimpleSetUp()
        {
            EntityArchetype entityArchetype = m_Manager.CreateArchetype(ComponentType.ReadWrite<UnitRuntime>(),
                ComponentType.ReadWrite<Translation>(),
                ComponentType.ReadWrite<Rotation>());

            _e = m_Manager.CreateEntity(entityArchetype);

            m_Manager.SetComponentData(_e, new UnitRuntime
            {
                storedStartingAction =  5,
            });

            World.CreateSystem<UnitQueueSystem>().Update();
        }

        [Test]
        public void UnitRuntimeEcsSimplePasses()
        {
            UnitRuntime unitRuntime = m_Manager.GetComponentData<UnitRuntime>(_e);
            Assert.That(unitRuntime.storedStartingAction, Is.Not.Zero);
            Assert.That(unitRuntime.actionTime, Is.False);
        }

        [Test]
        public void UnitQueueEcsTestSimplePasses()
        {
            Assert.IsTrue(m_Manager.Exists(_e));
            Assert.IsTrue(m_Manager.HasComponent(_e, ComponentType.ReadWrite<UnitRuntime>()));
        }

        //TODO Doesn't work halt ECS and cause a memory leak. Figure out a solution.
        //I Want it to wait for the storedStartingAction duration since there is a action that happens when the condition is satisfied.
        /*[UnityTest]
        public IEnumerator UnitQueueEcsTestWithEnumeratorPasses()
        {
            UnitRuntime unitRuntime = m_Manager.GetComponentData<UnitRuntime>(_e);

            yield return new WaitForSeconds(unitRuntime.storedStartingAction + Offset);


            Assert.That(unitRuntime.startingAction, Is.Zero);
            var nativeQueryEntityContainer  = m_Manager.CreateEntityQuery(ComponentType.ReadWrite<UnitExecutionRuntime>()).ToEntityArray(Allocator.TempJob);

            Assert.IsTrue(nativeQueryEntityContainer.IsCreated);
            Assert.That(nativeQueryEntityContainer.Length, Is.Not.Zero);

            nativeQueryEntityContainer.Dispose();
        }*/
    }
}