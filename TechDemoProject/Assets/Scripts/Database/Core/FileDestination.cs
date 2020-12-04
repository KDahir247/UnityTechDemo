using System;

namespace Tech.DB
{
    [Serializable]
    public enum FileDestination //To abstract Database builders with GlobalSettings
    {
        SkillPath,
        UnitPath,
        EquipmentPath,
        ItemPath,
        MaterialPath,
        EnemyPath
    }
}