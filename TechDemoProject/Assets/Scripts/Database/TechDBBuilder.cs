using System;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.Unity;
using MessagePack.Unity.Extension;
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
    
    public class TechDBBuilder
    {
        public TechDBBuilder(params IFormatterResolver[] resolvers)
        {
            try
            {
                StaticCompositeResolver.Instance.Register(new[]
                {
                    GeneratedResolver.Instance,
                    BuiltinResolver.Instance,
                    PrimitiveObjectResolver.Instance,
                    UnityResolver.Instance,
                    UnityBlitResolver.Instance,
                    MasterMemoryResolver.Instance,
                    StandardResolver.Instance,
                });
            }
            catch
            {
                // ignored
            }
        }

        public async UniTask Build([NotNull] Func<DatabaseBuilder,DatabaseBuilder> builderAction, string name)
        {
            DatabaseBuilder builder = new DatabaseBuilder();
            
            builder = builderAction.Invoke(builder);
            
            byte[] bufferData = builder.Build();

            var resourceDir = $"{Application.dataPath}/Resources";
            Directory.CreateDirectory(resourceDir);
            var fileName = $"/{name}.bytes";

            using (var fs = new FileStream(resourceDir + fileName, FileMode.Create))
            {
                await fs.WriteAsync(bufferData, 0, bufferData.Length);
            }
            AssetDatabase.Refresh();
        }
    }
}