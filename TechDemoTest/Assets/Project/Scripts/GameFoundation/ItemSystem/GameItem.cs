using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UniRx;
using UnityEngine.GameFoundation;
using ZLogger;

public sealed class GameItem : DataFoundation
{
    private readonly CompositeDisposable _disposable
        = new CompositeDisposable();

    private readonly List<InventoryItem> _initialStackableItems
        = new List<InventoryItem>();

    private readonly Subject<StackableInventoryItem> _itemSubject
        = new Subject<StackableInventoryItem>();

    private readonly ILogger _logger
        = LogManager.GetLogger<GameItem>();

    private readonly Dictionary<string, InventoryItem> _stackableItemsDictionary
        = new Dictionary<string, InventoryItem>();

    public GameItem()
    {
        try
        {
            RetrieveStackableData();
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e.Message);
            throw new Exception(e.Message);
        }
    }

    public IObservable<StackableInventoryItem> StackableItemValueChanged()
    {
        return _itemSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();

        _itemSubject.Dispose();

        if (!_disposable.IsDisposed)
            _disposable.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.inventory.itemQuantityChanged += StackableQuantityChanged;
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.inventory.itemQuantityChanged -= StackableQuantityChanged;
    }

    private void StackableQuantityChanged(IQuantifiable quantifiable, long _)
    {
        if (!(quantifiable is StackableInventoryItem item)) return;

        if (_stackableItemsDictionary.ContainsKey(item.definition.key))
            _itemSubject.OnNext(item);
    }

    private void RetrieveStackableData()
    {
        GameFoundationSdk
            .inventory
            .FindItems(item => item is StackableInventoryItem, _initialStackableItems);

        if (_initialStackableItems.Count <= 0) return;

        for (byte index = 0; index < _initialStackableItems.Count; index++)
            _stackableItemsDictionary.Add(_initialStackableItems[index].definition.key, _initialStackableItems[index]);
    }

    public void IncreaseQuantity([NotNull] string inventoryDefinitionKey)
    {
        var stackableItem = GetStackableInventoryItem(inventoryDefinitionKey);
        stackableItem?.SetQuantity(stackableItem.quantity + 1);
    }

    public void DecreaseQuantity([NotNull] string inventoryDefinitionKey)
    {
        var stackableItem = GetStackableInventoryItem(inventoryDefinitionKey);
        stackableItem?.SetQuantity(stackableItem.quantity - 1);
    }

    public void SetQuantity([NotNull] string inventoryDefinitionKey, int quantity)
    {
        var stackableItem = GetStackableInventoryItem(inventoryDefinitionKey);
        stackableItem?.SetQuantity(quantity);
    }

    public void ZeroQuantity([NotNull] string inventoryDefinitionKey)
    {
        var stackableItem = GetStackableInventoryItem(inventoryDefinitionKey);
        stackableItem?.SetQuantity(0);
    }

    [CanBeNull]
    private StackableInventoryItem GetStackableInventoryItem([NotNull] string key)
    {
        StackableInventoryItem stackableInventoryItem;

        if (_stackableItemsDictionary.ContainsKey(key))
        {
            stackableInventoryItem = _stackableItemsDictionary[key] as StackableInventoryItem;
        }
        else
        {
            var stackableDefinition = GameFoundationSdk.catalog.Find<InventoryItemDefinition>(key);
            stackableInventoryItem =
                GameFoundationSdk.inventory.CreateItem(stackableDefinition) as StackableInventoryItem;
            _stackableItemsDictionary.Add(key, stackableInventoryItem);
        }

        return stackableInventoryItem;
    }
}