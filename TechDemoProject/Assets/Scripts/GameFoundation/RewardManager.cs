using UnityEngine;
using UnityEngine.GameFoundation;

public sealed class RewardManager : MonoBehaviour
{
    private GameWallet game;
    private GameReward reward;
    [SerializeField] private bool ok;
    [SerializeField] private bool getBanned;

    void Start()
    {
       game = new GameWallet();
    }

    public void ClaimReward()
    {
        reward = new GameReward();
        reward.Claim("dailyReward");
    }
    private void Update()
    {
        GameFoundationSdk.rewards.Update(); //Refresh Reward states.
        
        //TODO testing UI remove
        if (ok)
        {
            game.AddToWallet("note", 2);
        }

        if (getBanned)
        {
            game.AddToWallet("cred", 3);
        }
    }
}