using System.Threading;
using Cysharp.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using ZLogger;

public sealed class SceneSystem
{
    private readonly ILogger logger = LogManager.GetLogger<SceneSystem>();
    private CancellationToken cancellationToken;

    public SceneSystem(CancellationToken cancellationToken)
    {
        this.cancellationToken = cancellationToken;
    }

    public async UniTask<SceneInstance> LoadSceneAsync(string sceneAddressName,
        LoadSceneMode loadSceneMode)
    {
        cancellationToken.Register(OperationCanceled);
        cancellationToken.ThrowIfCancellationRequested();

        var resourceLocation = await Addressables.LoadResourceLocationsAsync(sceneAddressName);
        var sceneInstance = await Addressables.LoadSceneAsync(resourceLocation[0], loadSceneMode);

        return sceneInstance;
    }

    public async UniTask UnloadScene(SceneInstance sceneInstance)
    {
        await Addressables.UnloadSceneAsync(sceneInstance);
    }

    private void OperationCanceled()
    {
        logger.ZLog(LogLevel.Debug, "Scene Loading has been canceled mid way.");
    }
}