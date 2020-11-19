using System;
using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using Tech.Network.Hub;
using Tech.Network.Param;

namespace Tech.Server.Hubs
{
    //TODO Create Helpers
    class NetworkHub : StreamingHubBase<INetworkHub, INetworkHubReceiver>, INetworkHub
    {
        private IGroup room;
        private IInMemoryStorage<Player> playerMemoryStorage;
        Player self;
       // private IInMemoryStorage<self> storage;

        public async Task<Player[]> JoinAsync(string username)
        {

            self = new Player(){Level = 0, Name = username};

            const string roomName = "Title";

            (room, playerMemoryStorage) = await Group.AddAsync(roomName, self);

            
            Console.WriteLine("joined");

            this.BroadcastToSelf(room)
                .OnJoin(self);

            return playerMemoryStorage.AllValues.ToArray();

        }

        public async Task LeaveAsync()
        {
            await room.RemoveAsync(this.Context);
            this.Broadcast(room).OnLeave(self);
        }

        public async Task DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public async Task TerminateAsync()
        {
             Program.Source.Cancel(false);
             Environment.Exit(0);   
        }


        protected override ValueTask OnConnecting()
        {

            Console.WriteLine($"Client the connected {this.Context.ContextId}");
            return CompletedTask;
        }

        protected override ValueTask OnDisconnected()
        {
            Console.WriteLine("Disconnected");

            return CompletedTask;
        }
    }
}
