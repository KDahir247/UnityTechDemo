using System;

namespace Tech.DB
{
    [Serializable]
    public enum FileDestination //To abstract Database builders with GlobalSettings
    {
        None = 0,
        UserPath,
        AbilityPath,
        SkillPath,
        UnitPath,
        EquipmentPath,
        ItemPath,
        MaterialPath,
        EnemyPath
    }
}