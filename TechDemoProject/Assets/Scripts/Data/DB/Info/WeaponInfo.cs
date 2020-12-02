using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct WeaponInfo
    {
        //TODO add passive to the weapon 
        public uint Level { get; set; }
        public Rarity Rarity { get; set; }
        public WeaponType WeaponType { get; set; }
        public Stat WeaponStat { get; set; }
        public Element ElementAttack { get; set; }
        public Ailment StatusAliment { get; set; }
        public uint NumberOfAttack { get; set; }
    }
}