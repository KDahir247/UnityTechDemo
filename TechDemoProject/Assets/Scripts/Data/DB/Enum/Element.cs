using System;

namespace Tech.Data.DB
{
    [Flags] //can contain multi-element
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum Element
    {
        Neutral,
        Fire,
        Water,
        Earth,
        Light,
        Dark
    }
}