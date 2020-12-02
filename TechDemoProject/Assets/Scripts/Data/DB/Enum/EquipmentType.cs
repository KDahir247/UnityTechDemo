using System;

namespace Tech.Data.DB
{
    [Serializable] //Just in case enum uses nonstandard enum values
    public enum EquipmentType
    {
        Head,
        Armor,
        Torso,
        Feet
    }
}