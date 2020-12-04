using System;
using MasterMemory;
using MessagePack;

namespace Tech.DB
{
    [MemoryTable("nill")]
    [MessagePackObject(true)]
    public class Nill : IMessagePackSerializationCallbackReceiver
    {
        
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public string Name { get; set; }
        
        [SecondaryKey(0)] public int Index { get; set; }

        public void OnBeforeSerialize()
        {
            //Called Before Serialization 

        }

        public void OnAfterDeserialize()
        {
            //Called After DeSerialization 
        }
    }
}