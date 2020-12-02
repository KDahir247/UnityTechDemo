using System;

namespace Tech.Data.DB
{
    [Flags] //can contain itemType
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum ItemType
    {
        None,
        Collectible,
        BurstHeal,
        HOT,
        Recovery,
        Revive,
        PhysicalMitigation,
        MagicalMitigation,
        DOTMitigation,
        InnocenceBoost,
        BuffRemoval,
        InstanceInnocenceFill,
        InnocenceEfficacy
    }
}