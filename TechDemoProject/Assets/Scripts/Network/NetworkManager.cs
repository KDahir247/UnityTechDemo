using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using Tech.Network.Hub;
using Tech.Network.Param;
using UnityEngine;

public class NetworkManager : MonoBehaviour, INetworkHubReceiver
{
    private Grpc.Core.Channel _channel;

    private INetworkHub _networkHub;
    
    // Start is called before the first frame update
    private async void Awake()
    {
        //uses port 2300
        _channel = new Grpc.Core.Channel("Localhost: 12345", ChannelCredentials.Insecure);
        _networkHub = StreamingHubClient.Connect<INetworkHub, INetworkHubReceiver>(this._channel, this);

        InitializeHub();
    }

    private async void InitializeHub()
    {
        var player = new Player()
        {
            Name = "Bob",
            // ID = Ulid.Empty, 
            Level = 0
        };

        await _networkHub.JoinAsync(player);
    }

    private async UniTaskVoid OnDestroy()
    {
        await _networkHub.DisposeAsync();
        await _channel.ShutdownAsync();
    }

    public void OnJoin(Player player)
    {
        Debug.Log("Sucessfully connected");
    }

    public void OnLeave(string name)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnect(string name)
    {
        throw new System.NotImplementedException();
    }
}
