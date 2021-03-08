using System;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using MessagePack.Resolvers;
using Tech.Core;
using UnityEditor;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.DB
{
    internal sealed class DynamicDbBuilder
    {
        private readonly ILogger _logger = LogManager.GetLogger<DynamicDbBuilder>();
        private readonly IStream _stream;
        private string _fileName = string.Empty;
        private ImmutableBuilder _immutableBuilder;

        public DynamicDbBuilder(IStream stream)
        {
            _stream = stream;
        }


        public void DynamicallyMutateDatabase(FileDestination fileDestination,
            [NotNull] Func<ImmutableBuilder, ImmutableBuilder> mutateDelegate)
        {
            try
            {
                var loadedImmutableBuilder = _stream.TryGetDatabase(fileDestination).ToImmutableBuilder();
                _immutableBuilder = mutateDelegate.Invoke(loadedImmutableBuilder);
                _fileName = $"{_stream.GetDatabaseFileName(fileDestination)}.bytes";
            }
            catch (Exception e)
            {
                _logger.ZLogCritical(
                    $"Either the specified fileDestination couldn't acquire the database file : {_stream.TryGetDatabase(fileDestination)} \n or stream is null : {_stream}");
                throw new NullReferenceException(e.Message);
            }
        }

        public async UniTask BuildToDatabaseAsync()
        {
            var memoryDatabase = _immutableBuilder.Build();
            var bytesBuffer = memoryDatabase.ToDatabaseBuilder(StaticCompositeResolver.Instance).Build();

            if (!Directory.Exists($"{Application.dataPath}/Resources"))
                Directory.CreateDirectory($"{Application.dataPath}/Resources");

            using var fileStream = new FileStream($"{Application.dataPath}/Resources/{_fileName}", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            await fileStream.WriteAsync(bytesBuffer, 0, bytesBuffer.Length);
            AssetDatabase.Refresh();
        }
    }
}