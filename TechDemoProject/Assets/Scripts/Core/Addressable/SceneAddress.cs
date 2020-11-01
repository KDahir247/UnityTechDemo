using System;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private static readonly ILogger Logger = LogManager.GetLogger("SceneLogger");

        private static bool _readyToLoad = true;

        private static SceneInstance _sceneInstance;

        public static void SceneLoad(
            string address,
            LoadSceneMode sceneMode = LoadSceneMode.Single,
            bool loadOnComplete = true)
        {
            if (address == string.Empty)
            {
                Logger.ZLogError("Address Empty Exception: Address passed in is equal to string.empty (\"\")");
                return;
            }

            try
            {
                if (_readyToLoad)
                    Addressables.LoadSceneAsync(address, sceneMode, loadOnComplete).Completed += OnSceneLoaded;
                else
                    Addressables.UnloadSceneAsync(_sceneInstance).Completed += OnSceneUnload;
            }
            catch (Exception e)
            {
                Logger.ZLogError(e.Message);
            }
        }

        public static async UniTask SceneLoadByNameOrLabel(string nameOrLabel,
            IProgress<float> progressLoadAddress = null,
            IProgress<float> progressLoadScene = null,
            IProgress<float> progressUnloadScene = null,
            CancellationToken cancellationTokenLoadAddress = default,
            CancellationToken cancellationTokenLoadScene = default,
            CancellationToken cancellationTokenUnloadScene = default)
        {
            if (nameOrLabel == string.Empty)
            {
                Logger.ZLogError(
                    "Name or Label Empty Exception: Label or name passed in is equal to string.empty (\"\")");
                return;
            }

            try
            {
                if (_readyToLoad)
                {
                    var resourceLocations =
                        await Addressables.LoadResourceLocationsAsync(nameOrLabel).ToUniTask(progressLoadAddress,
                            cancellationToken: cancellationTokenLoadAddress);

                    if (resourceLocations.Count <= 0)
                    {
                        Logger.ZLogError("Couldn't find Addressable Scene with the parameter passed through");
                        return;
                    }

                    _sceneInstance = await Addressables.LoadSceneAsync(resourceLocations[0])
                        .ToUniTask(progressLoadScene, PlayerLoopTiming.Update, cancellationTokenLoadScene);
                    _readyToLoad = false;
                }
                else
                {
                    await Addressables.UnloadSceneAsync(_sceneInstance).ToUniTask(progressUnloadScene,
                        PlayerLoopTiming.Update, cancellationTokenUnloadScene);
                    _readyToLoad = true;
                    _sceneInstance = new SceneInstance();
                }
            }
            catch (Exception e)
            {
                Logger.ZLogError(e.Message);
            }
        }

        public static void SceneLoad(IResourceLocation resourceLocation,
            LoadSceneMode sceneMode = LoadSceneMode.Single,
            bool loadOnComplete = true)
        {
            if (resourceLocation == null)
                Logger.ZLogError("Empty Resource Location : Resource Location passed through is null. ");
            try
            {
                if (_readyToLoad)
                    Addressables.LoadSceneAsync(resourceLocation, sceneMode, loadOnComplete).Completed +=
                        OnSceneLoaded;
                else
                    Addressables.UnloadSceneAsync(_sceneInstance).Completed += OnSceneUnload;
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
                var stringBuilder = new Utf8ValueStringBuilder();
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
                var stringBuilder = new Utf8ValueStringBuilder();
                stringBuilder.Append("Failed to load scene address");
                Logger.ZLogError(stringBuilder.ToString());
                stringBuilder.Dispose();
            }
        }
    }
}