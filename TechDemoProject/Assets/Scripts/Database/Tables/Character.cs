using System;
using MasterMemory;
using MessagePack;

namespace Tech.DB
{
    //TODO should be inheritance since there will be enemy and both character and enemy have similarity.
    [MemoryTable("character")]
    [MessagePackObject(true)]
    public class Character : IMessagePackSerializationCallbackReceiver
    {
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public string Name { get; set; }

        [SecondaryKey(0)] public int Index { get; set; }


        public string Description { get; set; }

        public void OnBeforeSerialize()
        {
            //Called Before Serialization
        }

        public void OnAfterDeserialize()
        {
            //Called After Deserialization
        }
    }
}