using System;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using Tech.Network.Hub;
using Tech.Network.Param;

namespace Tech.Server.Hubs
{
    class NetworkHub : StreamingHubBase<INetworkHub, INetworkHubReceiver>, INetworkHub
    {
        private IGroup room;
        Player player;

        public async Task JoinAsync(Player player)
        {
            const string roomName = "Title";
            room = await Group.AddAsync(roomName);

            this.player = player;
            Console.WriteLine("joined");
            this.BroadcastToSelf(room)
                .OnJoin(player);
        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);
            this.Broadcast(room).OnLeave(player.Name);
        }

        public async Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        protected override ValueTask OnConnecting()
        {

            Console.WriteLine($"Client the connected {this.Context.ContextId}");
            return CompletedTask;
        }

        protected override ValueTask OnDisconnected()
        {


            return CompletedTask;
        }
    }
}
