using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    private Grpc.Core.Channel _channel;

    // Start is called before the first frame update
    private void Awake()
    {
        //uses port 2300
        _channel = new Grpc.Core.Channel("localhost: 2300", ChannelCredentials.Insecure);
    }

    private async UniTask OnDestroy()
    {
        await _channel.ShutdownAsync();
    }
}
