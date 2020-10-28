using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using ZLogger;
using Object = UnityEngine.Object;

namespace Tech.Core
{
    public static class AssetAddress
    {
        private static readonly Microsoft.Extensions.Logging.ILogger Logger = LogManager.GetLogger("AssetLogger");
        
        public static async UniTaskVoid CreateAssetList<T>(AssetReference assetReference,
            IList<T> objects,
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

                objects.Add( await assetReference.InstantiateAsync()
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
            }
            else
            {
                Logger.ZLogInformation("AssetReference Asset is Null. \nCan't load nothing from the AssetAddress."); 
            }
        }

        public static async UniTaskVoid CreateAssetList<T>(IList<AssetReference> assetReferences,
            IList<T> objects,
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
                        .InstantiateAsync()
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

        public static async UniTaskVoid LoadByLocation<T>(IList<IResourceLocation> resourceLocations, IList<T> objects)
        where T : Object
        {
            if (resourceLocations.Count <= 0)
            {
                Logger.ZLogInformation("resourceLocation list is empty");
            }
            
            foreach (IResourceLocation location in resourceLocations)
            {
                objects.Add(await Addressables.InstantiateAsync(objects) as T);
            }
        }

        public static async UniTaskVoid LoadByNameOrLabel<T>(string nameOrLabel,
            IList<T> objects,
            IProgress<float> progress = null,
            CancellationToken cancellationToken = default(CancellationToken))
        where T : Object
        {
            IList<IResourceLocation> resourceLocations= await Addressables.LoadResourceLocationsAsync(nameOrLabel).ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);

           foreach (IResourceLocation location in resourceLocations)
           {
               objects.Add(await Addressables.InstantiateAsync(location).ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken) as T);
           }
        }

        public static void Release(IList<Object> objects)
        {
            foreach (var o in objects)
            {
                Addressables.Release(o);
            }
        }
        
        public static void Release(Object obj)
        {
            Addressables.Release(obj);
        }
    }
}