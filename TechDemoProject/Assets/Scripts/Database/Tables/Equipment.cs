using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [Serializable]
    [MemoryTable("equipment")]
    [MessagePackObject(true)]
    public class Equipment : IMessagePackSerializationCallbackReceiver
    {
        public byte[] Id { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        [PrimaryKey] public string Name { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        public string Address { get; set; }

        public string Description { get; set; }

        public byte[] ImageBytes { get; set; }

        public EquipmentInfo EquipmentInfo { get; set; }

        [SecondaryKey(2)] public int Index { get; set; }

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