using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;
using Random = UnityEngine.Random;

public class UICreateEntity : MonoBehaviour
{
    AssetSystem<GameObject> assetSystem = new AssetSystem<GameObject>(CancellationToken.None);
    private List<GameObject> collection = new List<GameObject>();
    public void CreateEntity()
    {
        assetSystem.LoadAsset(
            new AssetInfo("Human", new InstantiationParameters(new Vector3(Random.Range(-3,3),Random.Range(-1,1)), Quaternion.identity, null)), collection);
    }

}
