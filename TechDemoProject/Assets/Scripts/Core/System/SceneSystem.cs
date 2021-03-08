using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tech.Core;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using ZLogger;

internal sealed class SceneSystem
{
    private readonly ILogger _logger = LogManager.GetLogger<SceneSystem>();
    private CancellationToken _cancellationToken;

    public SceneSystem(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public async UniTask<SceneInstance> LoadSceneAsync(string sceneAddressName,
        LoadSceneMode loadSceneMode, [CanBeNull] Action onComplete)
    {
        _cancellationToken.Register(OperationCanceled);
        _cancellationToken.ThrowIfCancellationRequested();

        var resourceLocation = await Addressables.LoadResourceLocationsAsync(sceneAddressName);
        var sceneInstance = await Addressables.LoadSceneAsync(resourceLocation[0], loadSceneMode);
        onComplete?.Invoke();
        return sceneInstance;
    }

    public async UniTask UnloadScene(SceneInstance sceneInstance)
    {
        await Addressables.UnloadSceneAsync(sceneInstance);
    }

    private void OperationCanceled()
    {
        _logger?.ZLog(LogLevel.Debug, "Scene Loading has been canceled mid way.");
    }
}