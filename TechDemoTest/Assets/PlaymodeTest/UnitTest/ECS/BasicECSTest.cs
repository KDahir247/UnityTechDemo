using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Tests;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Random = UnityEngine.Random;

namespace Tech.Test.ECS
{

    [TestFixture]
    [Category("ECS TEST")]
    public class BasicECSTest : ECSTestsFixture
    {
        //Testing to see if works
        [Test]
        [Performance]
        public void BasicEcsTestSimplePasses()
        {
            using (Measure.Scope("ECS Basic Test"))
            {
                var entity =
                    m_Manager.CreateEntity(new ComponentType(typeof(Translation), ComponentType.AccessMode.ReadWrite));
                m_Manager.SetComponentData(entity, new Translation() {Value = new float3(1, 0, 0)});
                Assert.AreEqual(new float3(1, 0, 0), m_Manager.GetComponentData<Translation>(entity).Value);
                //Assert.AreEqual(float3.zero,  m_Manager.GetComponentData<Translation>(entity).Value); //intentionally failed to see if setup worked.
            }
        }

        [Test]
        [Performance]
        public void BasicEcsTestAddedSimplePasses()
        {
            using (Measure.Scope("ECS Basic Test"))
            {
                var entity =
                    m_Manager.CreateEntity(new ComponentType(typeof(UnitRuntime), ComponentType.AccessMode.ReadWrite));

                //Assert.AreEqual(float3.zero,  m_Manager.GetComponentData<Translation>(entity).Value); //intentionally failed to see if setup worked.

                World.CreateSystem<UnitQueueSystem>().Update();
                var entity2 =
                    m_Manager.CreateEntity(new ComponentType(typeof(UnitRuntime), ComponentType.AccessMode.ReadWrite));
            }
        }
    }
}