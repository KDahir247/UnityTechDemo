using UnityEngine;

namespace Experimental.Network
{
    public class ClientSend : MonoBehaviour
    {
        private static void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.tcp.SendData(packet);
        }

        private static void SendUDPData(Packet packet)
        {
            packet.WriteLength();
            Client.instance.udp.SendData(packet);
        }

        #region Packets

        public static void WelcomeReceived()
        {
            using (var packet = new Packet((int) ClientPackets.WelcomeReceived))
            {
                packet.Write(Client.instance.myId);
                packet.Write("Boby");

                SendTCPData(packet);
            }
        }

        #endregion
    }
}