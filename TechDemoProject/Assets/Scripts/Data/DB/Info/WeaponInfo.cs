using System;
using MessagePack;

namespace Tech.Data.DB
{
    [Serializable]
    [MessagePackObject(true)]
    public struct WeaponInfo
    {
        //TODO add passive to the weapon 
        public int Level { get; set; }
        public Rarity Rarity { get; set; }
        public WeaponType WeaponType { get; set; }
        public Stat WeaponStat { get; set; }
        public Element ElementAttack { get; set; }
        public Ailment StatusAliment { get; set; }
        public int NumberOfAttack { get; set; }
    }
}