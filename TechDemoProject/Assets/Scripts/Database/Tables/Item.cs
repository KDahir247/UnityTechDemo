using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [MemoryTable("item")]
    [MessagePackObject(true)]
    public class Item : IMessagePackSerializationCallbackReceiver
    {
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public string Name { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        //passive

        [NonUnique] private Rarity Rarity { get; set; }

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