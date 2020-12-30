using System;
using System.IO;
using Cysharp.Threading.Tasks;
using MasterData;
using MessagePack.Resolvers;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public sealed class DynamicDbBuilder
{
    private readonly ILogger logger = LogManager.GetLogger<DynamicDbBuilder>();
    private readonly IStream stream;
    private string fileName = string.Empty;
    private ImmutableBuilder immutableBuilder;

    public DynamicDbBuilder(IStream stream)
    {
        this.stream = stream;
    }


    public void DynamicallyMutateDatabase(FileDestination fileDestination,
        Func<ImmutableBuilder, ImmutableBuilder> mutateDelegate)
    {
        try
        {
            var loadedImmutableBuilder = stream.TryGetDatabase(fileDestination).ToImmutableBuilder();
            immutableBuilder = mutateDelegate.Invoke(loadedImmutableBuilder);
            fileName = $"{stream.GetDatabaseFileName(fileDestination)}.bytes";
        }
        catch (Exception e)
        {
            logger.ZLogCritical(
                $"Either the specified fileDestination couldn't acquire the database file : {stream.TryGetDatabase(fileDestination)} \n or stream is null : {stream}");
            throw new NullReferenceException(e.Message);
        }
    }

    public async UniTask BuildToDatabaseAsync()
    {
        var memoryDatabase = immutableBuilder.Build();
        var bytesBuffer = memoryDatabase.ToDatabaseBuilder(StaticCompositeResolver.Instance).Build();

        if (!Directory.Exists($"{Application.dataPath}/Resources"))
            Directory.CreateDirectory($"{Application.dataPath}/Resources");

        using var fileStream = new FileStream($"{Application.dataPath}/Resources/{fileName}", FileMode.Open);
        await fileStream.WriteAsync(bytesBuffer, 0, bytesBuffer.Length);
    }
}