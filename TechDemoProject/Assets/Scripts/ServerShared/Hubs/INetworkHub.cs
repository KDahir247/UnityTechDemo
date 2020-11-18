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
}