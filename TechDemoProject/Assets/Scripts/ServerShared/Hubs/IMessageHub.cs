using System.Threading.Tasks;
using MagicOnion;
using Tech.Network.Param;

namespace Tech.Network.Hub
{
    //Client -> Server API
    public interface IMessageHub : IStreamingHub<IMessageHub, IMessageHubReceiver>
    {
        //Send a Message to everybody Global
        Task SendMessageAsync(Player sender, string msg);

        //Send Message to Everybody except self
        Task SendMessageExceptSelfAsync(Player sender, string msg);

        //Send Message to a single player
        Task SendMessageToAsync(Player sender, Player to, string msg);

        //Send Message to a collection of player excluding the excluders
        Task SendMessageExceptAsync(Player sender, string msg, params Player[] excluders);
    }
}