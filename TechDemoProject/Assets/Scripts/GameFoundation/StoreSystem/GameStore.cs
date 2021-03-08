using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tech.Core;
using UnityEngine.GameFoundation;
using ZLogger;

public sealed class GameStore : DataFoundation
{
    private readonly List<BaseTransaction> _baseTransactions
        = new List<BaseTransaction>(5);

    private readonly GameTransaction _gameTransaction
        = new GameTransaction();

    private readonly ILogger _logger
        = LogManager.GetLogger<GameStore>();

    private readonly Dictionary<string, Store> _storeDictionary
        = new Dictionary<string, Store>(5);

    private readonly List<Store> _stores
        = new List<Store>(5);

    public GameStore()
    {
        try
        {
            RetrieveStoreData();
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e.Message);
            throw new Exception(e.Message);
        }
    }

    private void RetrieveStoreData()
    {
        GameFoundationSdk
            .catalog
            .GetItems(_stores);

        if (_stores.Count <= 0) return;

        for (byte index = 0; index < _stores.Count; index++)
            _storeDictionary.Add(_stores[index].key, _stores[index]);
    }

    public void PurchaseFromStore([NotNull] string storeDescriptionKey,
        [NotNull] string transactionDescriptionKey)
    {
        if (!PrePurchaseCondition(storeDescriptionKey)) return;

        _storeDictionary[storeDescriptionKey]
            .GetStoreItems(_baseTransactions);

        PurchaseTransaction(transactionDescriptionKey);
    }

    private void PurchaseTransaction(string transactionDescriptionKey)
    {
        var exists =
            _baseTransactions.Exists(currentTransaction => currentTransaction.key == transactionDescriptionKey);

        if (exists)
            _gameTransaction.Purchase(transactionDescriptionKey); //Check if the MainThreadDispatcher is enabled.
    }

    private bool PrePurchaseCondition([NotNull] string storeDescriptionKey)
    {
        return _storeDictionary.ContainsKey(storeDescriptionKey);
    }

    [CanBeNull]
    public List<BaseTransaction> RetrieveStoreTransactions([NotNull] string storeDescriptionKey)
    {
        if (!_storeDictionary.ContainsKey(storeDescriptionKey)) return default;

        var storeTransactions = new List<BaseTransaction>(5);
        _storeDictionary[storeDescriptionKey].GetStoreItems(storeTransactions);
        return storeTransactions;
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();
        _gameTransaction.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
    }
}