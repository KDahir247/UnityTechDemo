using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable] //Just in case enum uses nonstandard enum values
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
        public Element ElementType { get; set; }
        public TraitType TraitType { get; set; }
        public TargetStat TargetParameter { get; set; }
        public ParameterType SkillType { get; set; }
        public Target SkillTarget { get; set; }
        public Ailment Ailment { get; set; }
        public uint ManapointCost { get; set; }
        public uint NumberOfHits { get; set; }
        public uint Duration { get; set; }
        public uint Amount { get; set; } //Damage or heal amount
        public Stat Buff { get; set; }
    }
}