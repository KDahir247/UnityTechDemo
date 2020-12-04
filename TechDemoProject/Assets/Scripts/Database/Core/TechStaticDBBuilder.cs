using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;
using MessagePack.Unity.Extension;
using Tech.Utility;
using UnityEditor;
using UnityEngine;

namespace Tech.DB
{
    /*
    GeneratedResolver.Instance,
    BuiltinResolver.Instance,
    PrimitiveObjectResolver.Instance,
    UnityResolver.Instance,
    UnityBlitResolver.Instance,
    MasterMemoryResolver.Instance,
    StandardResolver.Instance,
    */

    public sealed class TechStaticDBBuilder
    {
        private static byte[] _buildBytes;
        private static string _path;
        private DatabaseBuilder _builder = new DatabaseBuilder();

        public TechStaticDBBuilder() :
            this(GeneratedResolver.Instance,
                BuiltinResolver.Instance,
                PrimitiveObjectResolver.Instance,
                UnityResolver.Instance,
                UnityBlitResolver.Instance,
                MasterMemoryResolver.Instance,
                Cysharp.Serialization.MessagePack.UlidMessagePackResolver.Instance,
                StandardResolver.Instance,
                PrimitiveObjectResolver.Instance)
        {
        }

        public TechStaticDBBuilder(params IFormatterResolver[] resolvers)
        {
            try
            {
                StaticCompositeResolver.Instance.Register(resolvers);
            }
            catch
            {
                // ignored
            }
        }

        public async UniTask Initialize()
        {
            foreach (string pathValue in GlobalSetting.DataPath.Values)
            {
                
                _buildBytes = _builder.Build();
                var resourceDir = $"{Application.dataPath}/Resources";
                Directory.CreateDirectory(resourceDir);
                var fileName = $"/{pathValue}.bytes";
                
                using (var fs = new FileStream(resourceDir + fileName, FileMode.Create))
                {
                     await fs.WriteAsync(_buildBytes, 0, _buildBytes.Length);
                }

                AssetDatabase.Refresh();
                
            }
            
            
            
        }

        public async UniTask Build([NotNull] Func<DatabaseBuilder, DatabaseBuilder> builderAction,
            FileDestination fileDestination)
        {
            GlobalSetting.DataPath.TryGetValue(fileDestination, out _path);

            _builder = builderAction.Invoke(_builder);
            _buildBytes = _builder.Build();

            //TODO find a better solution to SequenceEqual to compare the bytes. Append to file if the byte doesn't exist else return
            if (File.Exists($"{Application.dataPath}/Resources/{_path}.bytes") &&
                _buildBytes.SequenceEqual(Resources.Load<TextAsset>(_path)?.bytes ?? new byte[1] {0xff}))
            {
                return;
            }


            var resourceDir = $"{Application.dataPath}/Resources";
            Directory.CreateDirectory(resourceDir);
            var fileName = $"/{_path}.bytes";


            using (var fs = new FileStream(resourceDir + fileName, FileMode.Create))
            {
                _builder.WriteToStream(fs);
                // await fs.WriteAsync(bufferData, 0, bufferData.Length);
            }

            AssetDatabase.Refresh();
        }
    }
}