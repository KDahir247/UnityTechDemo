using System;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public sealed class StaticDbBuilder
{
    private readonly ILogger logger = LogManager.GetLogger<StaticDbBuilder>();
    private readonly IStream stream;
    private string fileName = string.Empty;

    public StaticDbBuilder(IStream stream)
    {
        this.stream = stream;
    }

    public void StaticallyMutateDatabase(FileDestination destination,
        [NotNull] Func<DatabaseBuilder, DatabaseBuilder> mutateDelegate)
    {
        try
        {
            fileName = $"{stream.GetDatabaseFileName(destination)}.bytes";
            stream.Builder = mutateDelegate.Invoke(stream.Builder);
        }
        catch (Exception e)
        {
            logger.ZLogCritical($"Stream passed in the constructor was null : {stream}");
            throw new NullReferenceException(e.Message);
        }
    }

    public async UniTask BuildToDatabaseAsync()
    {
        var bytesBuffer = stream.Builder.Build();

        if (!Directory.Exists($"{Application.dataPath}/Resources"))
            Directory.CreateDirectory($"{Application.dataPath}/Resources");

        using var fileStream = new FileStream($"{Application.dataPath}/Resources/{fileName}", FileMode.Create);
        await fileStream.WriteAsync(bytesBuffer, 0, bytesBuffer.Length);
    }
}