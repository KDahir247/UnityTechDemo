using System;

namespace Tech.Data.DB
{
    [Flags] //Target stat from ability and skill can affect multi parameters
    [Serializable]
    public enum TargetStat
    {
        Health,
        Attack,
        Defence,
        Manapoint,
        Magic,
        Spirit,
        Speed
    }
}