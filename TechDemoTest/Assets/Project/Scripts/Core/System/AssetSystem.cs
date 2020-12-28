using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class EntityInfo
{
    public EntityInfo(string name, InstantiationParameters instantiationParameters)
    {
        Name = name;
        InstantiationParameters = instantiationParameters;
    }

    public readonly string Name;
    public readonly InstantiationParameters InstantiationParameters;
}

//TODO include Custom Logger
public sealed class AssetSystem<T>
    where T : UnityEngine.Object
{
    private UniTask<GameObject>[] _taskToComplete;
    
    public async UniTaskVoid LoadAsset([NotNull] EntityInfo entityInfo,
        IList<T> instantiatedContainer,
        IProgress<float> progress,
        CancellationToken cancellationToken)
    {
        cancellationToken.Register(OperationCanceled);
        
        cancellationToken.ThrowIfCancellationRequested();
        
        IList<IResourceLocation> resourceLocations = await Addressables
            .LoadResourceLocationsAsync(entityInfo.Name)
            .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);


        _taskToComplete = new UniTask<GameObject>[resourceLocations.Count];
        
        for (int i = 0; i < resourceLocations.Count; i++)
        {
            _taskToComplete[i] = Addressables.InstantiateAsync(resourceLocations[i], entityInfo.InstantiationParameters)
                .ToUniTask(progress, PlayerLoopTiming.Update, cancellationToken);
        }

        GameObject[] results = await UniTask.WhenAll(_taskToComplete);

        for (byte index = 0; index < results.Length; index++)
        {
            var gameObject = results[index];
            instantiatedContainer.Add(gameObject as T);
        }
    }
    
    public void UnloadAllAsset([NotNull] IEnumerable<T> releaseObjs)
    {
        foreach (var releaseObj in releaseObjs)
        {
            Addressables.Release(releaseObj);
        }
    }

    private void OperationCanceled()
    {
        Debug.Log("Asset Loading has been canceled mid way");
    }
}
