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
        public int ManapointCost { get; set; }
        public int NumberOfHits { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; } //Damage or heal amount
        public Stat Buff { get; set; }
    }
}