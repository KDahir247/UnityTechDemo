using NUnit.Framework;
using Unity.Entities;
using Unity.Entities.Tests;
using Unity.Mathematics;
using Unity.PerformanceTesting;
using Unity.Transforms;
namespace Tech.Test
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

    }
}