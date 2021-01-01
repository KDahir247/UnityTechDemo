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
        = new Dictionary<string, Currency>();

    private readonly Subject<Currency> _currencySubject = new Subject<Currency>();

    private readonly CompositeDisposable _disposable
        = new CompositeDisposable();

    private readonly ILogger logger = LogManager.GetLogger<GameWallet>();

    public GameWallet([NotNull] params string[] currencyKeys)
    {
        if (!GameFoundationSdk.IsInitialized)
            throw new Exception("Game Wallet requires GameFoundation to be initialized");

        try
        {
            RetrieveCurrenciesData(currencyKeys);
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            logger.ZLogCritical(e.Message);
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

    public override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.wallet.balanceChanged += WalletOnBalanceChanged;
    }

    public override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.wallet.balanceChanged -= WalletOnBalanceChanged;
    }

    private void WalletOnBalanceChanged(IQuantifiable quantifiable, long _)
    {
        if (!(quantifiable is Currency currency)) return;

        foreach (var keyValue in _currencyDictionary)
            if (currency.key == keyValue.Key)
                _currencySubject.OnNext(_currencyDictionary[keyValue.Key]);
    }

    public void WalletValueChanged(Action<Currency> onNext)
    {
        _currencySubject
            .Subscribe(onNext)
            .AddTo(_disposable);
    }

    private void RetrieveCurrenciesData([NotNull] string[] currencyKeys)
    {
        for (byte i = 0; i < currencyKeys.Length; i++)
            _currencyDictionary.Add(currencyKeys[i], GameFoundationSdk.catalog.Find<Currency>(currencyKeys[i]));
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