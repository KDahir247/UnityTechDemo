using System;
using MessagePack;
using MessagePack.Resolvers;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;

namespace DefaultNamespace
{
    //Serialization Support
    /*
     * 
     */
    
    public class ResolverSerializationTest
    {
        private StaticCompositeResolver _resolver;
        
        [SetUp]
        public void ResolverInitializationSetUp()
        {
            _resolver = StaticCompositeResolver.Instance;
        }

        //0.45 ms
        [Test, Performance]
        public void ResolverInitializationTestSimplePasses()
        {
            using (Measure.Scope())
            {
                
                //Compression Test
                Assert
                    .That(MessagePackSerializer.DefaultOptions.Compression,
                        Is.EqualTo(MessagePackCompression.Lz4BlockArray));
           
                //Resolver Test 
                Assert
                    .That(MessagePackSerializer.DefaultOptions.Resolver, Is.Not.Null.And.EqualTo(StaticCompositeResolver.Instance));
           
                //Security
                Assert
                    .That(MessagePackSerializer.DefaultOptions.Security, Is.Not.Null.And.EqualTo(MessagePackSecurity.TrustedData));
            
                // Use the Assert class to test conditions.
            }

        }

        //1.28 ms
        [Test, Performance]
        public void ResolverSerializationTestSimplePasses()
        {
            using (Measure.Scope())
            {

                Assert.DoesNotThrow(() =>
                {

                    MessagePackSerializer.Serialize(new Vector2(5, 2), MessagePackSerializer.DefaultOptions);

                    Int16 serializationPrimitive = 45;
                    byte[] primitiveByteBuffer = MessagePackSerializer.Serialize(serializationPrimitive,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(primitiveByteBuffer.Length, Is.Not.EqualTo(0));

                    Vector2 serializationVector = Vector2.right;
                    byte[] unityByteBuffer =
                        MessagePackSerializer.Serialize(serializationVector, MessagePackSerializer.DefaultOptions);

                    Assert.That(unityByteBuffer.Length, Is.Not.EqualTo(0));

                    Vector2[] blitSerializationVector = new[] {Vector2.one, Vector2.down};
                    byte[] unityBlitByteBuffer = MessagePackSerializer.Serialize(blitSerializationVector,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(unityBlitByteBuffer.Length, Is.Not.EqualTo(0));
                    
                    //One for Ulid

                });
            }
        }


        //67 ms
        [Test, Performance]
        public void ResolverDeserializationSimplePasses()
        {
            using (Measure.Scope())
            {
                Assert.DoesNotThrow(() =>
                {
                    MessagePackSerializer.Serialize(new Vector2(5, 2), MessagePackSerializer.DefaultOptions);

                    Int16 serializationPrimitive = 45;
                    byte[] primitiveByteBuffer = MessagePackSerializer.Serialize(serializationPrimitive,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(primitiveByteBuffer.Length, Is.Not.EqualTo(0));

                    Vector2 serializationVector = Vector2.right;
                    byte[] unityByteBuffer =
                        MessagePackSerializer.Serialize(serializationVector, MessagePackSerializer.DefaultOptions);

                    Assert.That(unityByteBuffer.Length, Is.Not.EqualTo(0));

                    Vector2[] blitSerializationVector = new[] {Vector2.one, Vector2.down};
                    byte[] unityBlitByteBuffer = MessagePackSerializer.Serialize(blitSerializationVector,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(unityBlitByteBuffer.Length, Is.Not.EqualTo(0));

                    Int16 deserializedPrimitive =
                        MessagePackSerializer.Deserialize<Int16>(primitiveByteBuffer,
                            MessagePackSerializer.DefaultOptions);

                    Assert.That(deserializedPrimitive, Is.EqualTo(serializationPrimitive));

                    Vector2 deserializedVector =
                        MessagePackSerializer.Deserialize<Vector2>(unityByteBuffer,
                            MessagePackSerializer.DefaultOptions);

                    //Vector Inherit IEquatable for comparision
                    Assert.That(deserializedVector, Is.EqualTo(serializationVector));

                    Vector2[] blitDeserializationVector =
                        MessagePackSerializer.Deserialize<Vector2[]>(unityBlitByteBuffer,
                            MessagePackSerializer.DefaultOptions);

                    Assert.That(blitDeserializationVector.Length, Is.EqualTo(blitSerializationVector.Length));
                    
                    for (byte i = 0; i < blitDeserializationVector.Length; i++)
                    {
                        Assert.AreEqual(blitDeserializationVector[i], blitSerializationVector[i]);
                    }
                });
            }
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityEngine.TestTools.UnityTest]
        public System.Collections.IEnumerator ResolverInitializationTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}