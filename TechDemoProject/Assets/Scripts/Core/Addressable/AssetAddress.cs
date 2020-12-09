using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Object = UnityEngine.Object;

//Script Happens Separately from ECS System 

//TODO look at later to see if any classes calling the method should await
namespace Tech.Core
{
    internal static class AssetAddress
    {
        private static readonly ILogger Logger = LogManager.GetLogger("AssetLogger");

        private static readonly List<UniTask<GameObject>> TaskList = new List<UniTask<GameObject>>();

        private static readonly CompositeDisposable Disposable = new CompositeDisposable();

        static AssetAddress()
        {
            Application.quitting += () =>
            {
                if (!Disposable.IsDisposed)
                    Disposable.Dispose();
            };
        }

        public static async UniTaskVoid CreateAssetList<T>([NotNull] AssetReference assetReference,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            [CanBeNull] IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            if (assetReference.editorAsset != null || assetReference.Asset != null)
            {
                cancellationToken.Register(OperationCanceled);

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

        public static async UniTaskVoid CreateAssetList<T>([NotNull] IEnumerable<AssetReference> assetReferences,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            [CanBeNull] IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            TaskList.Clear();

            cancellationToken.Register(OperationCanceled);

            foreach (var assetReference in assetReferences)
                if (assetReference.Asset != null || assetReference.editorAsset != null)
                    TaskList.Add(assetReference
                        .InstantiateAsync(instantiationParameters.Position,
                            instantiationParameters.Rotation,
                            instantiationParameters.Parent)
                        .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken));
                else
                    Logger.ZLogInformation(
                        "Current index AssetReference is empty (null) \n Can't load nothing from the AssetAddress.");

            var gameObjects = await UniTask.WhenAll(TaskList);

            foreach (var gameObject in gameObjects) objects.Add(gameObject as T);

            progress?.Report(1.0f);
        }

        public static async UniTaskVoid GetAllLocation(string label,
            IList<IResourceLocation> loadedLocation,
            [CanBeNull] IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.Register(OperationCanceled);

            var unloadLocation = await Addressables.LoadResourceLocationsAsync(label)
                .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);


            foreach (var location in unloadLocation) loadedLocation.Add(location);

            progress?.Report(1.0f);
        }

        public static async UniTaskVoid LoadByLocation<T>([NotNull] IList<IResourceLocation> resourceLocations,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            [CanBeNull] IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            if (resourceLocations.Count <= 0) Logger.ZLogInformation("resourceLocation list is empty");

            TaskList.Clear();

            cancellationToken.Register(OperationCanceled);

            foreach (var location in resourceLocations)
                TaskList.Add(Addressables
                    .InstantiateAsync(location,
                        instantiationParameters)
                    .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken));

            var gameObjects = await UniTask.WhenAll(TaskList);

            foreach (var gameObject in gameObjects) objects.Add(gameObject as T);

            progress?.Report(1.0f);
        }

        public static async UniTaskVoid LoadByNameOrLabel<T>(string nameOrLabel,
            IList<T> objects,
            InstantiationParameters instantiationParameters,
            [CanBeNull] IProgress<float> progress = null,
            CancellationToken cancellationToken = default)
            where T : Object
        {
            TaskList.Clear();

            cancellationToken.Register(OperationCanceled);

            var resourceLocations = await Addressables.LoadResourceLocationsAsync(nameOrLabel)
                .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);


            foreach (var location in resourceLocations)
                TaskList.Add(Addressables
                    .InstantiateAsync(location,
                        instantiationParameters)
                    .ToUniTask(progress,
                        PlayerLoopTiming.Update,
                        cancellationToken));

            var gameObjects = await UniTask.WhenAll(TaskList);


            foreach (var gameObject in gameObjects) objects.Add(gameObject as T);

            progress?.Report(1.0f);
        }

        public static void Release(IList<Object> objects, float timer = 0)
        {
            if (timer <= 0)
                foreach (var o in objects)
                    Addressables.Release(o);
            else
                Observable.Timer(TimeSpan.FromSeconds(timer))
                    .Subscribe(_ =>
                    {
                        foreach (var o in objects)
                            Addressables.Release(o);
                    })
                    .AddTo(Disposable);
        }

        public static void Release(Object obj, float timer = 0)
        {
            if (timer <= 0)
                Addressables.Release(obj);
            else
                Observable.Timer(TimeSpan.FromSeconds(timer))
                    .Subscribe(_ => Addressables.Release(obj))
                    .AddTo(Disposable);
        }


        private static void OperationCanceled()
        {
            Logger.ZLogError("Loading assets have been canceled mid way of loading");
        }
    }
}