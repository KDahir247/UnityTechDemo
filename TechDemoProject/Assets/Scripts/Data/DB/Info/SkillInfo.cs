using System;
using MessagePack;

namespace Tech.Data.DB
{
    public enum TraitType
    {
        Trait,
        Equip,
        Passive
    }

    [Serializable]
    [MessagePackObject(true)]
    public struct SkillInfo
    {
        public uint ManapointCost { get; set; }
        
        public TraitType TraitType { get; set; }
        
        public TargetStat TargetParameter { get; set; }
        
        public Element ElementType { get; set; }
        
        public ParameterType SkillType { get; set; }
        
        public Target SkillTarget { get; set; }
        
        public Ailment Ailment { get; set; }
        
        public uint NumberOfHits { get; set; }
        
        public uint Duration { get; set; }
        
        public uint Amount { get; set; } //Damage or heal amount
        
        public Stat Buff { get; set; }
    }
}