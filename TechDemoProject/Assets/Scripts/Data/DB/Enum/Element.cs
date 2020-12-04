using System;

namespace Tech.Data.DB
{
    [Flags]
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum Element
    {
        Neutral = 0,
        Fire = 1,
        Water = 2,
        Earth = 3,
        Light = 4,
        Dark = 5
    }
}