using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct EquipmentInfo
    {
        
        //TODO add a passive to the equipment
        public int Level { get; set; }
        public Rarity Rarity { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public Stat EquipmentStat { get; set; }
        public Element ElementalAttack { get; set; }
        public Ailment StatusAilment { get; set; }
    }
}