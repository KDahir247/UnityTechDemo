using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MasterData;
using MessagePack.Resolvers;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public sealed class DatabaseStream : IStream
{
    private readonly Dictionary<FileDestination, string> databaseFileReference
        = new Dictionary<FileDestination, string>
        {
            {FileDestination.None, ""},
            {FileDestination.TestDestination, "test-data"}
        };

    private readonly ILogger logger = LogManager.GetLogger<DatabaseStream>();
    public DatabaseBuilder Builder { get; set; }

    public DatabaseStream()
    {
        Builder = new DatabaseBuilder(StaticCompositeResolver.Instance);
    }

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