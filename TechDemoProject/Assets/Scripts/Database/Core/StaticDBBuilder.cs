using System;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using Tech.Core;
using UnityEditor;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.DB
{
    internal sealed class StaticDbBuilder
    {
        private readonly ILogger _logger = LogManager.GetLogger<StaticDbBuilder>();
        private readonly IStream _stream;
        private string _fileName = string.Empty;

        public StaticDbBuilder(IStream stream)
        {
            _stream = stream;
        }

        public void StaticallyMutateDatabase(FileDestination destination,
            [NotNull] Func<DatabaseBuilder, DatabaseBuilder> mutateDelegate)
        {
            try
            {
                _fileName = $"{_stream.GetDatabaseFileName(destination)}.bytes";
                _stream.Builder = mutateDelegate.Invoke(_stream.Builder);
            }
            catch (Exception e)
            {
                _logger.ZLogCritical($"Stream passed in the constructor was null : {_stream}");
                throw new NullReferenceException(e.Message);
            }
        }

        public async UniTask BuildToDatabaseAsync()
        {
            var bytesBuffer = _stream.Builder.Build();

            if (!Directory.Exists($"{Application.dataPath}/Resources"))
                Directory.CreateDirectory($"{Application.dataPath}/Resources");

            using var fileStream = new FileStream($"{Application.dataPath}/Resources/{_fileName}", FileMode.Create);
            await fileStream.WriteAsync(bytesBuffer, 0, bytesBuffer.Length);
            AssetDatabase.Refresh();
        }
    }
}