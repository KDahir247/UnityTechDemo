using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public sealed class SceneSystem
{
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
        Debug.Log("Scene Loading has been canceled mid way.");
    }
}