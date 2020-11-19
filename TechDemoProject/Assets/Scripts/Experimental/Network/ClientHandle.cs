using System.Net;
using UnityEngine;

namespace Experimental.Network
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet _packet)
        {
            var _msg = _packet.ReadString();
            var _myId = _packet.ReadInt();

            Debug.Log($"Message from server: {_msg}");
            Client.instance.myId = _myId;

            ClientSend.WelcomeReceived();

            Client.instance.udp.Connect(((IPEndPoint) Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void SpawnPlayer(Packet packet)
        {
            var id = packet.ReadInt();
            var username = packet.ReadString();
            var position = packet.ReadVector3();
            var rotation = packet.ReadQuaternion();

            GameManager.instance.SpawnPlayers(id, username, position, rotation);
        }
    }
}