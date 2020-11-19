using Tech.Network.Param;

namespace Tech.Network.Hub
{
    //Server -> Client API
    public interface IMessageHubReceiver
    {
        void ReceivedMessage(Player sender, string msg);
    }
}