using UnityEngine;
using UnityEngine.GameFoundation;

public sealed class RewardManager : MonoBehaviour
{
    private void Update()
    {
        GameFoundationSdk.rewards.Update(); //Refresh Reward states.
    }
}