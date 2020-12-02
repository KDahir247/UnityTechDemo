using System;

namespace Tech.Data.DB
{
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum UnitType
    {
        Attacker,
        Supporter,
        Healer,
        Defender,
        Versatile,
        Other
    }
}