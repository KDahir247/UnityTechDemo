using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UniRx;
using UnityEngine.GameFoundation;
using ZLogger;

public sealed class GameInventory : DataFoundation
{
    private readonly CompositeDisposable _disposable
        = new CompositeDisposable();

    private readonly List<InventoryItem> _inventoryItems
        = new List<InventoryItem>(5);

    private readonly List<InventoryItem> _inventoryItemsByDefinition
        = new List<InventoryItem>(5);

    private readonly Subject<(InventoryItem, List<InventoryItem>)> _inventorySubject
        = new Subject<(InventoryItem, List<InventoryItem>)>();

    private readonly ILogger _logger = LogManager.GetLogger<GameInventory>();

    public GameInventory()
    {
        try
        {
            RetrieveItemsData();
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e.Message);
        }
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();

        _inventorySubject.Dispose();

        if (!_disposable.IsDisposed)
            _disposable.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.inventory.itemAdded += InventoryItemQuantityChanged;
        GameFoundationSdk.inventory.itemDeleted += InventoryItemQuantityChanged;
    }

    public IObservable<(InventoryItem, List<InventoryItem>)> InventoryValueChanged()
    {
        return _inventorySubject
            .AddTo(_disposable)
            .AsObservable();
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.inventory.itemAdded -= InventoryItemQuantityChanged;
        GameFoundationSdk.inventory.itemDeleted -= InventoryItemQuantityChanged;
    }

    private void InventoryItemQuantityChanged(InventoryItem inventoryItem)
    {
        RetrieveItemsData();

        _inventorySubject
            .OnNext((inventoryItem, _inventoryItems)); //added/removed item, remaining items
    }

    private void RetrieveItemsData()
    {
        GameFoundationSdk
            .inventory
            .GetItems(_inventoryItems);
    }

    public void AddToInventory([NotNull] string inventoryDefinitionKey)
    {
        var inventoryDefinition = GameFoundationSdk.catalog.Find<InventoryItemDefinition>(inventoryDefinitionKey);

        GameFoundationSdk
            .inventory
            .CreateItem(inventoryDefinition);
    }

    public void RemoveInventory([NotNull] string inventoryDefinitionKey)
    {
        var inventoryItemDefinition =
            GameFoundationSdk.catalog.Find<InventoryItemDefinition>(inventoryDefinitionKey);

        var inventoryCount =
            GameFoundationSdk.inventory.FindItems(inventoryItemDefinition, _inventoryItemsByDefinition);

        if (inventoryCount > 0)
        {
            GameFoundationSdk.inventory.Delete(_inventoryItemsByDefinition[0]);

            _inventoryItemsByDefinition.RemoveAt(0);
        }
    }

    public void RemoveAllFromInventoryType([NotNull] string inventoryDefinitionKey)
    {
        var inventoryItemDefinition =
            GameFoundationSdk.catalog.Find<InventoryItemDefinition>(inventoryDefinitionKey);

        GameFoundationSdk.inventory.Delete(inventoryItemDefinition);
    }
}