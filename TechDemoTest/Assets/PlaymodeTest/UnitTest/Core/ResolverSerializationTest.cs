using System;
using System.Collections;
using MessagePack;
using MessagePack.Resolvers;
using NUnit.Framework;
using Unity.PerformanceTesting;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tech.Test
{
    public sealed class ResolverSerializationTest
    {
        private Ulid _ulid;

        [SetUp]
        public void ResolverInitializationSetUp()
        {
            _ulid = Ulid.NewUlid(DateTimeOffset.Now);
        }

        //0.45 ns
        [Test]
        [Performance]
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
                    .That(MessagePackSerializer.DefaultOptions.Resolver,
                        Is.Not.Null.And.EqualTo(StaticCompositeResolver.Instance));

                //Security
                Assert
                    .That(MessagePackSerializer.DefaultOptions.Security,
                        Is.Not.Null.And.EqualTo(MessagePackSecurity.TrustedData));

                // Use the Assert class to test conditions.
            }
        }

        //1.28 ns
        [Test]
        [Performance]
        public void ResolverSerializationTestSimplePasses()
        {
            using (Measure.Scope())
            {
                Assert.DoesNotThrow(() =>
                {
                    MessagePackSerializer.Serialize(new Vector2(5, 2), MessagePackSerializer.DefaultOptions);

                    short serializationPrimitive = 45;
                    var primitiveByteBuffer = MessagePackSerializer.Serialize(serializationPrimitive,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(primitiveByteBuffer.Length, Is.GreaterThan(0));

                    var serializationVector = Vector2.right;
                    var unityByteBuffer =
                        MessagePackSerializer.Serialize(serializationVector, MessagePackSerializer.DefaultOptions);

                    Assert.That(unityByteBuffer.Length, Is.GreaterThan(0));

                    Vector2[] blitSerializationVector = {Vector2.one, Vector2.down};
                    var unityBlitByteBuffer = MessagePackSerializer.Serialize(blitSerializationVector,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(unityBlitByteBuffer.Length, Is.GreaterThan(0));

                    //One for Ulid
                    var ulidByteBuffer = MessagePackSerializer.Serialize(_ulid, MessagePackSerializer.DefaultOptions);
                    Assert.That(ulidByteBuffer.Length, Is.GreaterThan(0));
                });
            }
        }


        //67 ns
        [Test]
        [Performance]
        public void ResolverDeserializationSimplePasses()
        {
            using (Measure.Scope())
            {
                Assert.DoesNotThrow(() =>
                {
                    MessagePackSerializer.Serialize(new Vector2(5, 2), MessagePackSerializer.DefaultOptions);

                    short serializationPrimitive = 45;
                    var primitiveByteBuffer = MessagePackSerializer.Serialize(serializationPrimitive,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(primitiveByteBuffer.Length, Is.GreaterThan(0));

                    var serializationVector = Vector2.right;
                    var unityByteBuffer =
                        MessagePackSerializer.Serialize(serializationVector, MessagePackSerializer.DefaultOptions);

                    Assert.That(unityByteBuffer.Length, Is.GreaterThan(0));

                    Vector2[] blitSerializationVector = {Vector2.one, Vector2.down};
                    var unityBlitByteBuffer = MessagePackSerializer.Serialize(blitSerializationVector,
                        MessagePackSerializer.DefaultOptions);

                    Assert.That(unityBlitByteBuffer.Length, Is.GreaterThan(0));

                    var ulidByteBuffer = MessagePackSerializer.Serialize(_ulid, MessagePackSerializer.DefaultOptions);

                    Assert.That(ulidByteBuffer.Length, Is.GreaterThan(0));

                    var deserializedPrimitive =
                        MessagePackSerializer.Deserialize<short>(primitiveByteBuffer,
                            MessagePackSerializer.DefaultOptions);

                    Assert.That(deserializedPrimitive, Is.EqualTo(serializationPrimitive));

                    var deserializedVector =
                        MessagePackSerializer.Deserialize<Vector2>(unityByteBuffer,
                            MessagePackSerializer.DefaultOptions);

                    //Vector Inherit IEquatable for comparision
                    Assert.That(deserializedVector, Is.EqualTo(serializationVector));

                    var blitDeserializationVector =
                        MessagePackSerializer.Deserialize<Vector2[]>(unityBlitByteBuffer,
                            MessagePackSerializer.DefaultOptions);

                    Assert.That(blitDeserializationVector.Length, Is.EqualTo(blitSerializationVector.Length));

                    for (byte i = 0; i < blitDeserializationVector.Length; i++)
                        Assert.AreEqual(blitDeserializationVector[i], blitSerializationVector[i]);

                    var deserializationUlid =
                        MessagePackSerializer.Deserialize<Ulid>(ulidByteBuffer, MessagePackSerializer.DefaultOptions);

                    Assert.AreEqual(deserializationUlid, _ulid);
                });
            }
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator ResolverInitializationTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}