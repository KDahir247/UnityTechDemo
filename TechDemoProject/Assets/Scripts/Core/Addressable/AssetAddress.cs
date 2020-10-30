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
using Object = UnityEngine.Object;

//Script Happens Separately from ECS System 

namespace Tech.Core
{
    public static class AssetAddress
    {
        private static readonly Microsoft.Extensions.Logging.ILogger Logger = LogManager.GetLogger("AssetLogger");

        //TODO Create a collection of disposable
        private static IDisposable _disposableEvent;
        private static IDisposable _disposableEvent1;
        
        public static async UniTaskVoid CreateAssetList<T>(AssetReference assetReference,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
            where T : Object
        {
            if (assetReference.editorAsset != null || assetReference.Asset != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Logger.ZLogError("Cancellation Exception: The AssetAddress Retrieval Operation was canceled.");
                    return;
                }

                objects.Add( await assetReference.InstantiateAsync(instantiationParameters.Position, instantiationParameters.Rotation, instantiationParameters.Parent)
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
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
            CancellationToken cancellationToken = default(CancellationToken))
        where T : Object
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Logger.ZLogError("Cancellation Exception: The AssetAddress Retrieval Operation was canceled.");
                return;
            }

            foreach (AssetReference assetReference in assetReferences)
            {
                if (assetReference.Asset != null || assetReference.editorAsset != null)
                {
                    objects.Add(await assetReference
                        .InstantiateAsync(instantiationParameters.Position, instantiationParameters.Rotation, instantiationParameters.Parent)
                        .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
                }
                else
                {
                    Logger.ZLogInformation("Current index AssetReference is empty (null) \n Can't load nothing from the AssetAddress.");
                }
            }
        }
        
        public static async UniTaskVoid GetAllLocation(string label,
            IList<IResourceLocation> loadedLocation,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Logger.ZLogError("Cancellation Exception: The AssetAddress Retrieval Operation was canceled");
            }

            IList<IResourceLocation> unloadLocation = await Addressables.LoadResourceLocationsAsync(label)
                .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);

            foreach (var location in unloadLocation)
            {
                loadedLocation.Add(location);
            }
        }

        public static async UniTaskVoid LoadByLocation<T>(IList<IResourceLocation> resourceLocations,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        where T : Object
        {
            if (resourceLocations.Count <= 0)
            {
                Logger.ZLogInformation("resourceLocation list is empty");
            }
            
            foreach (IResourceLocation location in resourceLocations)
            {
                objects.Add(await Addressables
                    .InstantiateAsync(objects,
                        instantiationParameters)
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
            }
        }

        public static async UniTaskVoid LoadByNameOrLabel<T>(string nameOrLabel,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        where T : Object
        {
            IList<IResourceLocation> resourceLocations= await Addressables.LoadResourceLocationsAsync(nameOrLabel).ToUniTask(Progress.Create<float>((f => Debug.Log(f))));

           foreach (IResourceLocation location in resourceLocations)
           {
               objects.Add(await Addressables
                   .InstantiateAsync(location,
                       instantiationParameters)
                   .ToUniTask(Progress.Create<float>((f => Debug.Log(f))), PlayerLoopTiming.Update, cancellationToken) as T);
           }
        }

        //TODO de-subscribe from event and make a collection to store the IDisposable
        public static void Release(IList<Object> objects, float timer = 0)
        {
            if (timer <= 0)
            {
                foreach (var o in objects)
                {
                    Addressables.Release(o);
                }
            }
            else
            {

                Observable.Timer(TimeSpan.FromSeconds(timer))
                    .Subscribe(_ =>
                    {
                        foreach (var o in objects)
                        {
                            Addressables.Release(o);
                        }
                    });
            }
        }
        
        public static void Release(Object obj, float timer = 0)
        {
            if (timer <= 0)
            {
                Addressables.Release(obj);
            }
            else
            {
                 Observable.Timer(TimeSpan.FromSeconds(timer))
                    .Subscribe(_ =>
                    {
                        Addressables.Release(obj);
                    });
            }
        }
    }
}