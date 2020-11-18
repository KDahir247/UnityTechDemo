using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tech.Network.Param;
namespace Tech.Network.Hub
{
    public interface INetworkHubReceiver
    {

        void OnJoin(Player player);

        void OnLeave(string name);

        void OnDisconnect(string name);
    }
}