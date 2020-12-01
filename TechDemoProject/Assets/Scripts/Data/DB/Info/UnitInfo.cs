using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct UnitInfo
    {
        public uint Level { get; set; }
        
        public Rarity Rarity { get; set; }
        
        public Stat Stat { get; set; }
        
        public RaceType RaceType { get; set; }
        
        public Element ElementAttack { get; set; }
        
        public Element ElementResistance { get; set; }
        
        public Ailment StatusAilmentResistance { get; set; }
        
        public WeaponType WeaponCompatibility { get; set; }
        
        public UnitType UnitType { get; set; }
        
    }
}