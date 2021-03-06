using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    //TODO should be inheritance since there will be enemy and both equipment and weapon have similarity.
    [Serializable]
    [MemoryTable("weapon")]
    [MessagePackObject(true)]
    public class Weapon : IMessagePackSerializationCallbackReceiver
    {
        public byte[] Id { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        [PrimaryKey] public string Name { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        public string Address { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        public WeaponInfo WeaponInfo { get; set; }

        [SecondaryKey(2)] public int Index { get; set; }


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