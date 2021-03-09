using UnityEngine;
using UnityEngine.GameFoundation;

public sealed class RewardManager : MonoBehaviour
{
    private GameWallet game;
    [SerializeField] private bool ok;
    [SerializeField] private bool getBanned;

    void Start()
    {
       game = new GameWallet();
    }
    
    private void Update()
    {
        GameFoundationSdk.rewards.Update(); //Refresh Reward states.

        //testing UI
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