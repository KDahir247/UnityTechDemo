using System;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using MessagePack;
using MessagePack.Resolvers;
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
                StaticCompositeResolver.Instance.Register(resolvers);
            }
            catch
            {
                // ignored
            }
        }

        public async UniTask Build([NotNull] Func<DatabaseBuilder, DatabaseBuilder> builderAction, string name)
        {
            var builder = new DatabaseBuilder();

            builder = builderAction.Invoke(builder);

            var bufferData = builder.Build();

            //TODO find a better solution to SequenceEqual to compare the bytes
            if (File.Exists($"{Application.dataPath}/Resources/{name}.bytes") &&
                bufferData.SequenceEqual(Resources.Load<TextAsset>(name)?.bytes ?? new byte[1] {0xff}))
            {
                Debug.Log("returning");
                return;
            }


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