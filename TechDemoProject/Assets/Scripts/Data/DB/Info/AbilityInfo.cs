using System;
using MessagePack;

namespace Tech.Data.DB
{
    //Unit Ultimate Ability
    [Serializable]
    [MessagePackObject(true)]
    public struct AbilityInfo
    {
        public uint InnocenceCost { get; set; } //unit's ultimate ability gauge cost. 
        
        public TargetStat TargetParameter { get; set; }

        public Element ElementType { get; set; }

        public ParameterType AbilityType { get; set; }

        public Target AbilityTarget { get; set; }
        
        public Ailment Ailment { get; set; }

        public uint NumberOfHits { get; set; }

        public uint Duration { get; set; }
        
        public uint Amount { get; set; } //Damage or heal amount
        
        public Stat Buff { get; set; }
        
    }
}