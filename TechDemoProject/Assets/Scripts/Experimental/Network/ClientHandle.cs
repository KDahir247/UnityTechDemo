using System.Net;
using UnityEngine;

namespace Experimental.Network
{
    public class ClientHandle: MonoBehaviour
    {

        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();
            
            Debug.Log($"Message from server: {_msg}");
            Client.instance.myId = _myId;
            
            ClientSend.WelcomeReceived();
            
            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void SpawnPlayer(Packet packet)
        {
            int id = packet.ReadInt();
            string username = packet.ReadString();
            Vector3 position = packet.ReadVector3();
            Quaternion rotation = packet.ReadQuaternion();
            
            GameManager.instance.SpawnPlayers(id, username, position, rotation);
        }
        
    }
}