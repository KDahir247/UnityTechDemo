using System;
using MessagePack;

namespace Tech.Data.DB
{
    //Unit Ultimate Ability
    [Serializable]
    [MessagePackObject(true)]
    public struct AbilityInfo
    {
        public Element ElementType { get; set; }
        public TargetStat TargetParameter { get; set; }
        public ParameterType AbilityType { get; set; }
        public Target AbilityTarget { get; set; }
        public Ailment Ailment { get; set; }
        public int NumberOfHits { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; } //Damage or heal amount
        public Stat Buff { get; set; }
    }
}