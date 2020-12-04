using System;
using System.IO;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using MasterData;
using Tech.Utility;
using UnityEditor;
using UnityEngine;

namespace Tech.DB
{
    public sealed class TechDynamicDBBuilder
    {
        private string _currentFileName;
        private MemoryDatabase _database;
        private ImmutableBuilder _immutableBuilder;
        private static bool _locked;
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
                Debug.Log(e.Message);
                _currentFileName = string.Empty;
                builder = null;
                return false;
            }
        }

        public async UniTask Build([NotNull] ImmutableBuilder immutableBuilder)
        {
            if (string.IsNullOrEmpty(_currentFileName)) return;
            var inMemoryDatabase = immutableBuilder.Build();
            var builder = inMemoryDatabase.ToDatabaseBuilder();
            var bufferBytes = builder.Build();

            var resourceDir = $"{Application.dataPath}/Resources";
            Directory.CreateDirectory(resourceDir);
            var fileName = $"/{_currentFileName}.bytes";

            await UniTask.WaitUntil(() => _locked == false); //prevent all entity from accessing the FileStream at the same time
            
            _locked = true;
            
            using (var fs = new FileStream(resourceDir + fileName, FileMode.Create))
            {
                await fs.WriteAsync(bufferBytes, 0, bufferBytes.Length).AsUniTask();
            }
            
            AssetDatabase.Refresh();
            _locked = false;
            
        }
    }
}