﻿using System;

namespace Tech.DB
{
    [Serializable]
    public enum FileDestination //To abstract Database builders with GlobalSettings
    {
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