using System;
using JetBrains.Annotations;
using MasterData;
using Tech.Core;
using Tech.Utility;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.DB
{
    internal static class TechDB
    {
        private static readonly ILogger Logger = LogManager.GetLogger("DatabaseLogger");

        [NotNull]
        internal static MemoryDatabase LoadDataBase(FileDestination fileDestination, bool internString = true)
        {
            if (GlobalSetting.DataPath.TryGetValue(fileDestination, out var path))
                return new MemoryDatabase(Resources.Load<TextAsset>(path).bytes, internString);
            Logger.ZLogError("Database hasn't been created");
            throw new Exception();
        }
    }
}