using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using MasterData;
using Tech.Core;
using Tech.Utility;
using UnityEditor;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tech.DB
{
    internal sealed class TechStaticDBBuilder
    {
        private static byte[] _buildBytes;
        private static string _path;

        private static readonly ILogger Logger = LogManager.GetLogger("StaticDBBuilder");
        private DatabaseBuilder _builder = new DatabaseBuilder();


        public void Initialize()
        {
            foreach (var pathValue in GlobalSetting.DataPath.Values)
            {
                _buildBytes = _builder.Build();
                var resourceDir = $"{Application.dataPath}/Resources";
                Directory.CreateDirectory(resourceDir);
                var fileName = $"/{pathValue}.bytes";

                using (var fs = new FileStream(resourceDir + fileName, FileMode.Create))
                {
                    _builder.WriteToStream(fs);
                }

                AssetDatabase.Refresh();
            }
        }

        internal void Build([NotNull] Func<DatabaseBuilder, DatabaseBuilder> builderAction,
            FileDestination fileDestination)
        {
            GlobalSetting.DataPath.TryGetValue(fileDestination, out _path);

            _builder = builderAction.Invoke(_builder);
            _buildBytes = _builder.Build();

            //TODO find a better solution to SequenceEqual to compare the bytes. Append to file if the byte doesn't exist else return
            if (File.Exists($"{Application.dataPath}/Resources/{_path}.bytes") &&
                _buildBytes.SequenceEqual(Resources.Load<TextAsset>(_path)?.bytes ?? new byte[1] {0xff}))
                return;

            var resourceDir = $"{Application.dataPath}/Resources";

            if (!Directory.Exists(resourceDir))
                Directory.CreateDirectory(resourceDir);

            var fileName = $"/{_path}.bytes";

            using (var fs = new FileStream(resourceDir + fileName, FileMode.Create))
            {
                _builder.WriteToStream(fs);
            }

            AssetDatabase.Refresh();
        }
    }
}