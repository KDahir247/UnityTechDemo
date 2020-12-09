using System;
using System.IO;
using JetBrains.Annotations;
using MasterData;
using MessagePack;
using MessagePack.Resolvers;
using Tech.Core;
using Tech.Utility;
using UnityEditor;
using UnityEngine;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.DB
{
    //TODO clean up
    internal sealed class TechDynamicDBBuilder
    {
        private static readonly ILogger Logger = LogManager.GetLogger("DynamicDBBuilder");

        //Option for Ulid support
        private readonly MessagePackSerializerOptions _options;
        private DatabaseBuilder _builder;
        private string _currentFileName;

        private MemoryDatabase _database;
        private ImmutableBuilder _immutableBuilder;

        public TechDynamicDBBuilder()
        {
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(StaticCompositeResolver.Instance)
                .WithCompression(MessagePackCompression.Lz4BlockArray);
        }

        public bool TryLoadDatabase(FileDestination destination, out ImmutableBuilder builder)
        {
            try
            {
                GlobalSetting
                    .DataPath
                    .TryGetValue(destination, out _currentFileName);


                _immutableBuilder = TechDB
                    .LoadDataBase(destination)
                    .ToImmutableBuilder();

                builder = _immutableBuilder;
                return true;
            }
            catch (Exception e)
            {
                Logger.ZLogError(e.Message);
                _currentFileName = string.Empty;
                builder = null;
                return false;
            }
        }

        public byte[] RegisterUlid(in Ulid ulid)
        {
            return MessagePackSerializer.Serialize(ulid, _options);
        }

        public Ulid UnRegisterUlid(in byte[] byteBuffer)
        {
            return MessagePackSerializer.Deserialize<Ulid>(byteBuffer, _options);
        }

        public void Build([NotNull] ImmutableBuilder immutableBuilder)
        {
            if (string.IsNullOrEmpty(_currentFileName)) return;
            _database = immutableBuilder.Build();
            _builder = _database.ToDatabaseBuilder();
            _builder.Build();

            var resourceDir = $"{Application.dataPath}/Resources";

            if (!Directory.Exists(resourceDir))
                Directory.CreateDirectory(resourceDir);

            var fileName = $"/{_currentFileName}.bytes";

            using (var fs = new FileStream($"{resourceDir}{fileName}", FileMode.Create))
            {
                _builder.WriteToStream(fs);
            }

            AssetDatabase.Refresh();
        }
    }
}