using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Object = UnityEngine.Object;

//Script Happens Separately from ECS System 

namespace Tech.Core
{
    public static class AssetAddress
    {
        private static readonly ILogger Logger = LogManager.GetLogger("AssetLogger");
        
        private static readonly CompositeDisposable Disposable = new CompositeDisposable();
        
        static AssetAddress()
        {
            Application.quitting += () =>
            {
                if(!Disposable.IsDisposed)
                    Disposable.Dispose();
            };
        }
        
        public static async UniTaskVoid CreateAssetList<T>(AssetReference assetReference,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            if (assetReference.editorAsset != null || assetReference.Asset != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Logger.ZLogError("Cancellation Exception: The AssetAddress Retrieval Operation was canceled.");
                    return;
                }

                objects.Add(await assetReference.InstantiateAsync(instantiationParameters.Position,
                        instantiationParameters.Rotation, instantiationParameters.Parent)
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
                
                progress?.Report(1.0f);
            }
            else
            {
                Logger.ZLogError("AssetReference Asset is Null. \nCan't load nothing from the AssetAddress.");
            }
        }

        public static async UniTaskVoid CreateAssetList<T>(IList<AssetReference> assetReferences,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Logger.ZLogError("Cancellation Exception: The AssetAddress Retrieval Operation was canceled.");
                return;
            }

            foreach (var assetReference in assetReferences)
                if (assetReference.Asset != null || assetReference.editorAsset != null)
                {

                    objects.Add(await assetReference
                        .InstantiateAsync(instantiationParameters.Position, instantiationParameters.Rotation,
                            instantiationParameters.Parent)
                        .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
                    
                    progress?.Report(1.0f);
                }

                else
                    Logger.ZLogInformation(
                        "Current index AssetReference is empty (null) \n Can't load nothing from the AssetAddress.");
        }

        public static async UniTaskVoid GetAllLocation(string label,
            IList<IResourceLocation> loadedLocation,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
                Logger.ZLogError("Cancellation Exception: The AssetAddress Retrieval Operation was canceled");

            var unloadLocation = await Addressables.LoadResourceLocationsAsync(label)
                .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);
            
            progress?.Report(1.0f);
            
            foreach (var location in unloadLocation) loadedLocation.Add(location);
        }

        public static async UniTaskVoid LoadByLocation<T>(IList<IResourceLocation> resourceLocations,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            if (resourceLocations.Count <= 0) Logger.ZLogInformation("resourceLocation list is empty");

            foreach (var location in resourceLocations)
            {
                objects.Add(await Addressables
                    .InstantiateAsync(objects,
                        instantiationParameters)
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
                
                progress?.Report(1.0f);
            }
        }
        
        public static async UniTaskVoid LoadByNameOrLabel<T>(string nameOrLabel,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            var resourceLocations = await Addressables.LoadResourceLocationsAsync(nameOrLabel)
                .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);

            progress?.Report(1);
            
            foreach (var location in resourceLocations)
            {
                objects.Add(await Addressables
                    .InstantiateAsync(location,
                        instantiationParameters)
                    .ToUniTask(Progress.Create<float>(f => Debug.Log(f)), PlayerLoopTiming.Update,
                        cancellationToken) as T);
                
                progress?.Report(1.0f);
            }
        }

        //TODO de-subscribe from event and make a collection to store the IDisposable
        public static void Release(IList<Object> objects, float timer = 0)
        {
            
            if (timer <= 0)
                foreach (var o in objects)
                    Addressables.Release(o);
            else
            {
                var disposible = Observable.Timer(TimeSpan.FromSeconds(timer))
                    .Subscribe(_ =>
                    {
                        foreach (var o in objects)
                        {
                            Addressables.Release(o);
                        }
                    }).AddTo(Disposable);
                
                
            }
        }

        public static void Release(Object obj, float timer = 0)
        {
            if (timer <= 0)
                Addressables.Release(obj);
            else
            {
                Observable.Timer(TimeSpan.FromSeconds(timer))
                    .Subscribe(_ => { Addressables.Release(obj); }).AddTo(Disposable);
            }
        }
    }
}