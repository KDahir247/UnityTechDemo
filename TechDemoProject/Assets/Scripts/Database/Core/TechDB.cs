using System;
using JetBrains.Annotations;
using MasterData;
using Tech.Utility;
using UnityEngine;

namespace Tech.DB
{
    public static class TechDB
    {
        private static FileDestination _previousDestination;
        private static MemoryDatabase _database;

        [NotNull]
        public static MemoryDatabase LoadDataBase(FileDestination fileDestination, bool internString = true)
        {
            if (GlobalSetting.DataPath.TryGetValue(fileDestination, out var path))
            {
                _database = new MemoryDatabase(Resources.Load<TextAsset>(path).bytes, internString);
                return _database;
            }

            throw new Exception("Database hasn't been created");
        }
    }
}