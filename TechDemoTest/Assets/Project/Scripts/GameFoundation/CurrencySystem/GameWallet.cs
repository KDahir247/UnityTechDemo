using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UniRx;
using UnityEngine.GameFoundation;
using ZLogger;

public sealed class GameWallet : DataFoundation
{
    private readonly Dictionary<string, Currency> _currencyDictionary
        = new Dictionary<string, Currency>(5);

    private readonly Subject<Currency> _currencySubject
        = new Subject<Currency>();

    private readonly CompositeDisposable _disposable
        = new CompositeDisposable();

    private readonly List<Currency> _initialCurrencies
        = new List<Currency>(5);

    private readonly ILogger _logger = LogManager.GetLogger<GameWallet>();

    public GameWallet()
    {
        try
        {
            RetrieveCurrenciesData();
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e.Message);
            throw new Exception();
        }
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();

        _currencySubject.Dispose();

        if (!_disposable.IsDisposed)
            _disposable.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.wallet.balanceChanged += WalletOnBalanceChanged;
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.wallet.balanceChanged -= WalletOnBalanceChanged;
    }

    private void WalletOnBalanceChanged(IQuantifiable quantifiable, long _)
    {
        if (!(quantifiable is Currency currency)) return;

        if (_currencyDictionary.ContainsKey(currency.key))
            _currencySubject.OnNext(_currencyDictionary[currency.key]);
    }

    public IObservable<Currency> WalletValueChanged()
    {
        return _currencySubject
            .AddTo(_disposable)
            .AsObservable();
    }

    private void RetrieveCurrenciesData()
    {
        GameFoundationSdk.catalog.GetItems(_initialCurrencies);

        for (byte currencyIndex = 0; currencyIndex < _initialCurrencies.Count; currencyIndex++)
            _currencyDictionary.Add(_initialCurrencies[currencyIndex].key, _initialCurrencies[currencyIndex]);
    }

    public long GetWalletBalance([NotNull] string walletKey)
    {
        if (!_currencyDictionary.ContainsKey(walletKey)) return 0;

       return GameFoundationSdk
            .wallet
            .Get(_currencyDictionary[walletKey]);
    }

    public void AddToWallet([NotNull] string walletKey, int amount)
    {
        if (!_currencyDictionary.ContainsKey(walletKey)) return;

        GameFoundationSdk
            .wallet
            .Add(_currencyDictionary[walletKey], amount);
    }

    public void RemoveFromWallet([NotNull] string walletKey, int amount)
    {
        if (!_currencyDictionary.ContainsKey(walletKey)) return;

        GameFoundationSdk
            .wallet
            .Remove(_currencyDictionary[walletKey], amount);
    }

    public void ResetWalletAmount([NotNull] string walletKey)
    {
        if (!_currencyDictionary.ContainsKey(walletKey)) return;

        GameFoundationSdk
            .wallet
            .Set(_currencyDictionary[walletKey], 0);
    }
}