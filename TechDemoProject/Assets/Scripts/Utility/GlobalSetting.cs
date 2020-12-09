using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Tech.DB;

//Container for Global Parameter.
//Test File. File hasn't been finalized 
namespace Tech.Utility
{
    public static class GlobalSetting
    {
        public static bool EnableVerbosityUnitaskBootstrap;
        public static bool EnableVerbosityState;

        //Maybe make a dictonary 

        [NotNull] private static readonly Dictionary<FileDestination, string> _dataPath =
            new Dictionary<FileDestination, string>
            {
                {FileDestination.AbilityPath, "ability-data"},
                {FileDestination.SkillPath, "skill-data"},
                {FileDestination.UnitPath, "unit-data"},
                {FileDestination.EquipmentPath, "equip-data"},
                {FileDestination.ItemPath, "item-data"},
                {FileDestination.MaterialPath, "mat-data"},
                {FileDestination.EnemyPath, "enemy-data"},
                {FileDestination.UserPath, "user-data"}
            };

        [NotNull] internal static readonly ReadOnlyDictionary<FileDestination, string> DataPath =
            new ReadOnlyDictionary<FileDestination, string>(_dataPath);
    }
}