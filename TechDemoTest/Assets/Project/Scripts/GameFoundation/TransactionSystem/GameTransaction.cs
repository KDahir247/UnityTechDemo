using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
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

    private readonly Subject<BaseTransaction> _transactionInitializedSubject
        = new Subject<BaseTransaction>();

    private readonly Subject<(int, int)> _transactionStepSubject
        = new Subject<(int, int)>();

    private readonly Dictionary<string, VirtualTransaction> _virtualDictionary
        = new Dictionary<string, VirtualTransaction>();

    private readonly List<VirtualTransaction> _virtualTransactions
        = new List<VirtualTransaction>();

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

        for (byte i = 0; i < _virtualTransactions.Count; i++)
            _virtualDictionary.Add(_virtualTransactions[i].key, _virtualTransactions[i]);
    }

    public void Purchase([NotNull] string virtualTransactionKey)
    {
        if (!_virtualDictionary.ContainsKey(virtualTransactionKey)) return;

        if (!MainThreadDispatcher.IsInitialized)
            MainThreadDispatcher.Initialize();

        MainThreadDispatcher
            .StartCoroutine(ExecuteTransaction(_virtualDictionary[virtualTransactionKey]));
    }

    private IEnumerator ExecuteTransaction([NotNull] BaseTransaction virtualTransaction)
    {
        using var deferred = GameFoundationSdk.transactions.BeginTransaction(virtualTransaction);
        while (!deferred.isDone) yield return null;
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();

        _transactionInitializedSubject.Dispose();
        _transactionStepSubject.Dispose();
        _transactionCompletedSubject.Dispose();
        _transactionFailedSubject.Dispose();

        if (!_disposable.IsDisposed)
            _disposable.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.transactions.transactionInitiated += TransactionInitiated;
        GameFoundationSdk.transactions.transactionProgressed += TransactionProgressing;
        GameFoundationSdk.transactions.transactionFailed += TransactionFailed;
        GameFoundationSdk.transactions.transactionSucceeded += TransactionCompleted;
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.transactions.transactionInitiated -= TransactionInitiated;
        GameFoundationSdk.transactions.transactionProgressed -= TransactionProgressing;
        GameFoundationSdk.transactions.transactionFailed -= TransactionFailed;
        GameFoundationSdk.transactions.transactionSucceeded -= TransactionCompleted;
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

    public IObservable<(int, int)> OnTransactionProgress()
    {
        return _transactionStepSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    public IObservable<BaseTransaction> OnTransactionInitiated()
    {
        return _transactionInitializedSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    private void TransactionCompleted(BaseTransaction transaction, TransactionResult transactionResult)
    {
        _transactionCompletedSubject.OnNext(transactionResult);
    }

    private void TransactionFailed(BaseTransaction transaction, Exception exceptionThrown)
    {
        _transactionFailedSubject.OnNext(exceptionThrown);
    }

    private void TransactionProgressing(BaseTransaction transaction, int currentStep, int totalStep)
    {
        _transactionStepSubject.OnNext((currentStep, totalStep));
    }

    private void TransactionInitiated(BaseTransaction transaction)
    {
        _transactionInitializedSubject.OnNext(transaction);
    }
}