using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tech.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using ZLogger;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public readonly struct AssetInfo
{
    internal readonly string AssetAddressName;
    internal readonly InstantiationParameters InstantiationParameters;

    public AssetInfo(string assetAddressName, InstantiationParameters instantiationParameters)
    {
        AssetAddressName = assetAddressName;
        InstantiationParameters = instantiationParameters;
    }
}

public sealed class AssetSystem<T>
    where T : Object
{
    private readonly ILogger _logger = LogManager.GetLogger<AssetSystem<T>>();
    private CancellationToken _cancellationToken;

     public AssetSystem(CancellationToken token)
    {
        _cancellationToken = token;
    }

    public void LoadAsset(AssetInfo assetInfo,
        [NotNull] IList<T> loadAssetContainer)
    {
        _cancellationToken.Register(OperationCanceled);
        _cancellationToken.ThrowIfCancellationRequested();

        InstantiateAsset(assetInfo, loadAssetContainer)
            .Forget();
    }

    private async UniTaskVoid InstantiateAsset(AssetInfo assetInfo,
        [NotNull] ICollection<T> loadAssetContainer)
    {
        var resourceLocations = await Addressables
            .LoadResourceLocationsAsync(assetInfo.AssetAddressName)
            .ToUniTask(cancellationToken: _cancellationToken);

        var gameObject =  await Addressables.InstantiateAsync(resourceLocations[0], assetInfo.InstantiationParameters)
            .ToUniTask(cancellationToken: _cancellationToken);

        loadAssetContainer.Add(gameObject as T);
    }

    public void UnloadAllAsset([NotNull] IEnumerable<T> releaseObjs)
    {
        foreach (var releaseObj in releaseObjs) Addressables.Release(releaseObj);
    }

    public void UnloadAsset(T objToRelease)
    {
        Addressables.Release(objToRelease);
    }

    private void OperationCanceled()
    {
        _logger?.ZLog(LogLevel.Warning, "Asset Loading has been canceled mid way");
    }
}