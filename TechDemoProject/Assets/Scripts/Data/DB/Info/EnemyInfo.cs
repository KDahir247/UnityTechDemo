using System;
using MessagePack;
using Tech.DB;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct EnemyInfo
    {
        public uint Level { get; set; } 
        
        public Rarity Rarity { get; set; }
        
        public Stat Stat { get; set; }
        
        public RaceType RaceType { get; set; }
        
        public Element ElementAttack { get; set; }
        
        public Element ElementResistance { get; set; }
        
        public Ailment StatusAilmentResistance { get; set; }
        
        //Loot
        // public Weapon[] WeaponLoots { get; set; }
        // public Equipment[] EquipmentLoots { get; set; }
        // public Item[] ItemLoots { get; set; }
        // public Material[] MaterialsLoots { get; set; }
    }
}