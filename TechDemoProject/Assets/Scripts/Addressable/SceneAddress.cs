using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using ZLogger;


//Script Happens Separately from ECS System 

namespace Tech.Core
{
    public static class SceneAddress
    {
        private static readonly Microsoft.Extensions.Logging.ILogger Logger = LogManager.GetLogger("SceneLogger");
        private static bool _readyToLoad = true;
        private static SceneInstance _sceneInstance;

        public static void SceneLoad(string address, LoadSceneMode sceneMode = LoadSceneMode.Single, bool loadOnComplete = true)
        {
            if (_readyToLoad)
            {
                Addressables.LoadSceneAsync(address, sceneMode, loadOnComplete, 100).Completed += OnSceneLoaded;
            }
            else
            {
                Addressables.UnloadSceneAsync(_sceneInstance, true).Completed += OnSceneUnload;
            }
        }

        private static void OnSceneUnload(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _readyToLoad = true;
                _sceneInstance = new SceneInstance();
            }
            else
            {
                Utf8ValueStringBuilder stringBuilder = new Utf8ValueStringBuilder(true);
                stringBuilder.AppendFormat("Failed to unload scene address \n [{0}] [{1}] [{2}]", obj.Result.Scene.buildIndex, obj.Result.Scene.name, obj.Result.Scene.path);
                Logger.ZLogError(stringBuilder.ToString());
            }
        }

        private static void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _readyToLoad = false;
                _sceneInstance = obj.Result;
            }
            else
            {
                Utf8ValueStringBuilder stringBuilder = new Utf8ValueStringBuilder(true);
                stringBuilder.AppendFormat("Failed to load scene address \n [{0}] [{1}] [{2}] [{3}]",obj.Result.Scene.buildIndex, obj.Result.Scene.name, obj.Result.Scene.path );
                Logger.ZLogError(stringBuilder.ToString());
            }
        }
    }
}