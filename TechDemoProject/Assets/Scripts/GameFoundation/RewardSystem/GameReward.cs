using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Tech.Core;
using UniRx;
using UnityEngine.GameFoundation;
using ZLogger;

public sealed class GameReward : DataFoundation
{
    private readonly CompositeDisposable _disposable
        = new CompositeDisposable();

    private readonly ILogger _logger =
        LogManager.GetLogger<GameReward>();

    private readonly Subject<Payout> _rewardCompletedSubject
        = new Subject<Payout>();

    private readonly Dictionary<string, Reward> _rewardDictionary
        = new Dictionary<string, Reward>();

    private readonly Subject<(string, Exception)> _rewardFailedSubject
        = new Subject<(string, Exception)>();

    private readonly List<Reward> _rewards
        = new List<Reward>(5);

    public GameReward()
    {
        try
        {
            RetrieveRewardData();
            SubscribeToGameFoundationEvent();
        }
        catch (Exception e)
        {
            _logger.ZLogCritical(e.Message);
            throw new Exception(e.Message);
        }
    }

    private void RetrieveRewardData()
    {
        GameFoundationSdk.rewards.GetRewards(_rewards);

        if (_rewards.Count <= 0) return;

        for (byte index = 0; index < _rewards.Count; index++)
            _rewardDictionary.Add(_rewards[index].key, _rewards[index]);
    }

    public void Claim([NotNull] string rewardKey)
    {
        if (!_rewardDictionary.ContainsKey(rewardKey))
            return;

        if (!MainThreadDispatcher.IsInitialized)
            MainThreadDispatcher.Initialize();

        MainThreadDispatcher.StartCoroutine(ClaimReward(_rewardDictionary[rewardKey]));
    }

    private IEnumerator ClaimReward([NotNull] Reward reward)
    {
        if (reward.IsInCooldown()) yield break; //already redeemed and on cooldown

        var claimableKey = reward.GetLastClaimableRewardItemKey();

        using var deferredResult = GameFoundationSdk.rewards.Claim(reward.rewardDefinition, claimableKey);
        while (!deferredResult.isDone) yield return null;
    }

    public override void Dispose()
    {
        UnSubscribeToGameFoundationEvent();

        _rewardCompletedSubject.Dispose();
        _rewardFailedSubject.Dispose();

        if (!_disposable.IsDisposed)
            _disposable.Dispose();
    }

    protected override void SubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.rewards.rewardItemClaimSucceeded += RewardItemClaimCompleted;
        GameFoundationSdk.rewards.rewardItemClaimFailed += RewardItemClaimFailed;
    }

    protected override void UnSubscribeToGameFoundationEvent()
    {
        GameFoundationSdk.rewards.rewardItemClaimSucceeded -= RewardItemClaimCompleted;
        GameFoundationSdk.rewards.rewardItemClaimFailed += RewardItemClaimFailed;
    }

    public IObservable<Payout> OnClaimingRewardCompleted()
    {
        return _rewardCompletedSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    public IObservable<(string, Exception)> OnClaimingRewardFailed()
    {
        return _rewardFailedSubject
            .AddTo(_disposable)
            .AsObservable();
    }

    private void RewardItemClaimCompleted([NotNull] Reward reward, [NotNull] string rewardItemKey, Payout payout)
    {
        _rewardCompletedSubject.OnNext(payout);
    }

    private void RewardItemClaimFailed(string rewardKey, string rewardItemKey, Exception exception)
    {
        _rewardFailedSubject.OnNext((rewardKey, exception));
    }
}