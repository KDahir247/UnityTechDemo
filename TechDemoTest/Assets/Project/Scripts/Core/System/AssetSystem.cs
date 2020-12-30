﻿using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger logger = LogManager.GetLogger<AssetSystem<T>>();
    private CancellationToken cancellationToken;

    public AssetSystem(CancellationToken token)
    {
        cancellationToken = token;
    }

    public void LoadAsset(AssetInfo assetInfo,
        [NotNull] IList<T> loadAssetContainer)
    {
        cancellationToken.Register(OperationCanceled);
        cancellationToken.ThrowIfCancellationRequested();

        InstantiateAsset(assetInfo, loadAssetContainer)
            .Forget();
    }

    private async UniTaskVoid InstantiateAsset(AssetInfo assetInfo,
        [NotNull] ICollection<T> loadAssetContainer)
    {
        var resourceLocations = await Addressables
            .LoadResourceLocationsAsync(assetInfo.AssetAddressName)
            .ToUniTask(cancellationToken: cancellationToken);

        var gameObject = await Addressables.InstantiateAsync(resourceLocations[0], assetInfo.InstantiationParameters)
            .ToUniTask(cancellationToken: cancellationToken);

        loadAssetContainer.Add(gameObject as T);
    }

    public void UnloadAllAsset([NotNull] IEnumerable<T> releaseObjs)
    {
        foreach (var releaseObj in releaseObjs) Addressables.Release(releaseObj);
    }

    private void OperationCanceled()
    {
        logger.ZLog(LogLevel.Warning, "Asset Loading has been canceled mid way");
    }
}