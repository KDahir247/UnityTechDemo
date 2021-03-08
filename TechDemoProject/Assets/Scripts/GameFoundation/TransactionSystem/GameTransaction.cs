using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tech.Core;
using UniRx;
using UnityEngine.GameFoundation;
using ZLogger;

public sealed class GameTransaction : DataFoundation
{
    private readonly CompositeDisposable _disposable
        = new CompositeDisposable();

    private readonly ILogger _logger
        = LogManager.GetLogger<GameTransaction>();

    private readonly Subject<TransactionResult> _transactionCompletedSubject
        = new Subject<TransactionResult>();

    private readonly Subject<Exception> _transactionFailedSubject
        = new Subject<Exception>();

    private readonly Dictionary<string, VirtualTransaction> _virtualDictionary
        = new Dictionary<string, VirtualTransaction>();

    private readonly List<VirtualTransaction> _virtualTransactions
        = new List<VirtualTransaction>(5);

    public GameTransaction()
    {
        try
        {
            ReceiveTransactionData();
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e.Message);
            throw new Exception(e.Message);
        }
    }

    private void ReceiveTransactionData()
    {
        GameFoundationSdk.catalog.GetItems(_virtualTransactions);

        if (_virtualTransactions.Count <= 0) return;

        for (byte index = 0; index < _virtualTransactions.Count; index++)
            _virtualDictionary.Add(_virtualTransactions[index].key, _virtualTransactions[index]);
    }

    public void Purchase([NotNull] string virtualTransactionKey)
    {
        if (!_virtualDictionary.ContainsKey(virtualTransactionKey)) return;

        if (!MainThreadDispatcher.IsInitialized)
            MainThreadDispatcher.Initialize();

        MainThreadDispatcher.StartCoroutine(ExecuteTransaction(_virtualDictionary[virtualTransactionKey]));
    }

    private IEnumerator ExecuteTransaction(VirtualTransaction virtualTransaction)
    {
        using var deferred = GameFoundationSdk.transactions.BeginTransaction(virtualTransaction);
        while (!deferred.isDone) yield return null;
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();

        _transactionCompletedSubject.Dispose();
        _transactionFailedSubject.Dispose();

        if (!_disposable.IsDisposed)
            _disposable.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.transactions.transactionSucceeded += TransactionCompleted;
        GameFoundationSdk.transactions.transactionFailed += TransactionFailed;
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.transactions.transactionSucceeded -= TransactionCompleted;
        GameFoundationSdk.transactions.transactionFailed -= TransactionFailed;
    }

    public IObservable<TransactionResult> OnTransactionCompleted()
    {
        return _transactionCompletedSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    public IObservable<Exception> OnTransactionFailed()
    {
        return _transactionFailedSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    private void TransactionCompleted(BaseTransaction transaction, TransactionResult transactionResult)
    {
        _transactionCompletedSubject.OnNext(transactionResult);
    }

    private void TransactionFailed(BaseTransaction transaction, Exception exception)
    {
        _transactionFailedSubject.OnNext(exception);
    }
}