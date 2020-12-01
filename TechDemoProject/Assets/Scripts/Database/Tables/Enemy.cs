using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [MemoryTable("enemy"), MessagePackObject(true)]
    public class Enemy : IMessagePackSerializationCallbackReceiver
    {
        [IgnoreMember] public Ulid Id { get; set; }
        
        [PrimaryKey] public string Name { get; set; }
        
        [SecondaryKey(0)] public int Index { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        public EnemyInfo EnemyInfo { get; set; }
        
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

