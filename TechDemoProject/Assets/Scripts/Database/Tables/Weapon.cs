using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    //TODO should be inheritance since there will be enemy and both equipment and weapon have similarity.
    [MemoryTable("weapon")]
    [MessagePackObject(true)]
    public class Weapon : IMessagePackSerializationCallbackReceiver
    {
        [IgnoreMember] public Ulid Id { get; set; }

        [PrimaryKey] public string Name { get; set; }

        [NonUnique] public Stat Stat { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        [SecondaryKey(0)] [NonUnique] public uint Level { get; set; }

        [SecondaryKey(1)] [NonUnique] public WeaponType WeaponType { get; set; }

        [SecondaryKey(2)] public int Index { get; set; }

        [NonUnique] public Rarity Rarity { get; set; }

        //Passive 
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