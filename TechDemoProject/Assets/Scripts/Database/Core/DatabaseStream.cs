using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MasterData;
using MessagePack.Resolvers;
using Tech.Core;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.DB
{
    public sealed class DatabaseStream : IStream
    {
        private readonly Dictionary<FileDestination, string> databaseFileReference
            = new Dictionary<FileDestination, string>
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

        private readonly ILogger logger = LogManager.GetLogger<DatabaseStream>();

        public DatabaseStream()
        {
            Builder = new DatabaseBuilder(StaticCompositeResolver.Instance);
        }

        public DatabaseBuilder Builder { get; set; }

        [NotNull]
        public MemoryDatabase TryGetDatabase(FileDestination fileDestination)
        {
            try
            {
                return new MemoryDatabase(Resources.Load<TextAsset>(databaseFileReference[fileDestination]).bytes);
            }
            catch (NullReferenceException e)
            {
                logger.ZLogCritical(
                    $"Database file hasn't been created yet for : {databaseFileReference[fileDestination]} located in Assets/Resources");
                throw new NullReferenceException(e.Message);
            }
        }

        public string GetDatabaseFileName(FileDestination fileDestination)
        {
            return databaseFileReference[fileDestination];
        }
    }
}