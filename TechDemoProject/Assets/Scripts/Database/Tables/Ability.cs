using System;
using MasterMemory;
using MessagePack;
using Tech.Data.DB;

namespace Tech.DB
{
    [Serializable]
    [MemoryTable("ability")]
    [MessagePackObject(true)]
    public class Ability : IMessagePackSerializationCallbackReceiver
    {
        public byte[] Id { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        [PrimaryKey] public string Name { get; set; }

        public int InnocenceCost { get; set; } //unit's ultimate ability gauge cost.

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        public string Address { get; set; }

        public string Description { get; set; }

        public string AbilityDescription { get; set; }

        public byte[] ImageBytes { get; set; }

        [NonUnique] public AbilityInfo AbilityInfo { get; set; }

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