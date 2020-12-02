using System;

namespace Tech.Data.DB
{
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum Target
    {
        Self,
        Target,
        AOETarget,
        Ally,
        AOEAlly
    }
}