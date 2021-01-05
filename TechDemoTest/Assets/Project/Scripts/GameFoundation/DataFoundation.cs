using System;
using System.Collections;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.GameFoundation;
using UnityEngine.GameFoundation.DefaultLayers;
using UnityEngine.Promise;

public abstract class DataFoundation : IDisposable
{
    protected DataFoundation()
    {
        if (!GameFoundationSdk.IsInitialized)
            throw new Exception("Game Wallet requires GameFoundation to be initialized");
    }

    public abstract void Dispose();
    protected abstract void SubscribeToGameFoundationEvent();
    protected abstract void UnSubscribeToGameFoundationEvent();

    public void Save()
    {
        if (!(GameFoundationSdk.dataLayer is PersistenceDataLayer dataLayer))
            return;

        TrackSaveProgress(dataLayer);
    }

    private void TrackSaveProgress([NotNull] PersistenceDataLayer dataLayer)
    {
        using var saveOperation = dataLayer.Save();

        if (!saveOperation.isDone)
            MainThreadDispatcher.StartUpdateMicroCoroutine(WaitForSaveCompletion(saveOperation));
    }

    private IEnumerator WaitForSaveCompletion(Deferred saveOperation)
    {
        var saveOperationWaitStatus = saveOperation.Wait().ToYieldInstruction();

        if (!saveOperationWaitStatus.IsDone) yield return null;
    }

    //TODO for later
    /*public void DeleteAllFile()
    {
        //UnSubscribe from GameFoundation event

        if(!(GameFoundationSdk.dataLayer is PersistenceDataLayer dataLayer))
            return;

        if(!(dataLayer.persistence is LocalPersistence localPersistence))
            return;

        UnSubscribeToGameFoundationEvent();
    }*/
}