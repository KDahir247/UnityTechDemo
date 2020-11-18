using System.Threading.Tasks;
using MagicOnion;
using Tech.Network.Param;

//real-time communication
namespace Tech.Network.Hub
{
    //Client -> Server API
    public interface INetworkHub : IStreamingHub<INetworkHub, INetworkHubReceiver>
    {
        Task JoinAsync(Player player);

        Task LeaveAsync();

        Task DisconnectAsync();
    }


    //Client API
    public interface INetworkHubReceiver
    {
        void OnJoin(Player player);

        void OnLeave(string name);

        void OnDisconnect(string name);
    }
}