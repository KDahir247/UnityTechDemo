using System.Collections.Generic;
using Tech.Data.Scriptable;

//Container for Global Parameter.
//Test File. File hasn't been finalized 

namespace Tech.Utility
{
    //TODO fix
    public static class GlobalSetting
    {
        //TODO remove
        public static readonly Dictionary<string, CharacterData> StoredCharacter =
            new Dictionary<string, CharacterData>();


        public static bool EnableVerbosityUnitaskBootstrap;
        public static bool EnableVerbosityState;


        //Used for database
        public static string SkillDataPath = "skill-data";
        public static string CharacterDataPath = "character-data";
        public static string EquipmentDataPath = "equip-data";
        public static string ItemDataPath = "item-data";
        public static string MaterialDataPath = "mat-data";
    }
}