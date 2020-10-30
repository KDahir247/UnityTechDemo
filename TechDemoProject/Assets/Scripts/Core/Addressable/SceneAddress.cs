using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
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

        public static void SceneLoad(
            string address,
            LoadSceneMode sceneMode = LoadSceneMode.Single,
            bool loadOnComplete = true)
        {
            if (address == String.Empty)
            {
                Logger.ZLogError("Address Empty Exception: Address passed in is equal to string.empty (\"\")");
                return;
            }

            try
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
            catch (Exception e)
            {
                Logger.ZLogError(e.Message);
            }
        }
        
        
        public static async UniTask SceneLoadByNameOrLabel(string nameOrLabel,
            IProgress<float> progressLoadAddress = null,
            IProgress<float> progressLoadScene = null,
            CancellationToken cancellationTokenLoadAddress = default(CancellationToken),
            CancellationToken cancellationTokenLoadScene = default(CancellationToken))
        {
            IList<IResourceLocation> resourceLocations =
                await Addressables.
                    LoadResourceLocationsAsync(nameOrLabel).ToUniTask(progressLoadAddress, PlayerLoopTiming.Update, cancellationTokenLoadAddress);

            if (resourceLocations.Count <= 0)
            {
                Logger.ZLogError("Couldn't find Addressable Scene with the parameter passed through");
                return;
            }

            await Addressables.LoadSceneAsync(resourceLocations[0], LoadSceneMode.Single, true).ToUniTask(progressLoadScene, PlayerLoopTiming.Update, cancellationTokenLoadScene);
        }
        
        public static void SceneLoad(IResourceLocation resourceLocation,
            LoadSceneMode sceneMode = LoadSceneMode.Single,
            bool loadOnComplete = true)
        {
            if (resourceLocation == null)
            {
                Logger.ZLogError("Empty Resource Location : Resource Location passed through is null. ");
            }
            try
            {
                if (_readyToLoad)
                {
                    Addressables.LoadSceneAsync(resourceLocation, sceneMode, loadOnComplete, 100).Completed +=
                        OnSceneLoaded;
                }
                else
                {
                    Addressables.UnloadSceneAsync(_sceneInstance).Completed += OnSceneUnload;
                }
            }
            catch (Exception e)
            {
                Logger.ZLogError(e.Message);
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
                Utf8ValueStringBuilder stringBuilder = new Utf8ValueStringBuilder();
                stringBuilder.Append("Failed to load scene address");
                Logger.ZLogError(stringBuilder.ToString());
                stringBuilder.Dispose();
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
                Utf8ValueStringBuilder stringBuilder = new Utf8ValueStringBuilder();
                stringBuilder.Append("Failed to load scene address");
                Logger.ZLogError(stringBuilder.ToString());
                stringBuilder.Dispose();
            }
        }
    }
}