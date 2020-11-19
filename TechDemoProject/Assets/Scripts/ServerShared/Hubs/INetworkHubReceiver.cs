using Tech.Network.Param;

namespace Tech.Network.Hub
{
    public interface INetworkHubReceiver
    {
        void OnJoin(Player player);

        void OnLeave(Player player);

        void OnDisconnect(Player player);
    }
}