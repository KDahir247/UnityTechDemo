using System;
using MasterMemory;
using MessagePack;

namespace Tech.DB
{
    [MemoryTable("user")]
    [MessagePackObject(true)]
    public class User : IMessagePackSerializationCallbackReceiver
    {
        //Ulid
        public byte[] Id { get; set; }

        [PrimaryKey]
        public int Level { get; set; }

        [StringComparisonOption(StringComparison.InvariantCultureIgnoreCase)]
        [SecondaryKey(0)]
        public string Username { get; set; }

        public byte[] TrophyTextureImage { get; set; }

        public int Note { get; set; } //fake currency

        public int Cred { get; set; } //real currency 

        public int Energy { get; set; }

        public string Comment { get; set; }

        public Unit[] PossessedUnit { get; set; }

        public Equipment[] PossessedEquipments { get; set; }

        public TechMaterial[] PossessedMaterials { get; set; }

        public Item[] PossessedItems { get; set; }

        public Weapon[] PossessedWeapons { get; set; }

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