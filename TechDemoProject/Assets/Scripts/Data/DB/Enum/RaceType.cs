using System;

namespace Tech.Data.DB
{
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum RaceType
    {
        Human,
        Beast,
        Dragon,
        Demon,
        Machine,
        Spirit,
        God
    }
}