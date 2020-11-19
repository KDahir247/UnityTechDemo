using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using Microsoft.Extensions.Logging;
using Pixelplacement;
using Tech.Core;
using Tech.Network.Hub;
using Tech.Network.Param;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using ZLogger;
using Channel = Grpc.Core.Channel;
using Debug = UnityEngine.Debug;

namespace Tech.Network
{
//TODO Modularize script. Script should only be responsible to launching server and connecting and disconnection
//TODO and storing all the players connected.
    public class NetworkManager : Singleton<NetworkManager>, INetworkHubReceiver
    {
        //Event Hooks
        public static readonly Subject<Unit> OnServerDisconnect = new Subject<Unit>();
        public static readonly BoolReactiveProperty ConnectedToNetwork = new BoolReactiveProperty(false);


        //Network Properties
        private Channel _channel;
        private INetworkHub _networkHub;

        //Containers
        private ReactiveDictionary<string, GameObject> _players = new ReactiveDictionary<string, GameObject>();
        private Process _serverProcess;

        private Task NetworkTest;

        //Properties
        [FormerlySerializedAs("Verbose")] [SerializeField]
        private bool verbose;

        public void OnJoin(Player player)
        {
            Debug.Log("Sucessfully connected");
        }

        public void OnLeave(Player player)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnect(Player player)
        {
            throw new NotImplementedException();
        }

        // Start is called before the first frame update
        private async void Awake()
        {
            InternetConnectionTest();

            InitializeServerConsole();
            ServerConsoleProcessEvent();

            _channel = new Channel("Localhost: 12345", ChannelCredentials.Insecure);
            _networkHub = StreamingHubClient.Connect<INetworkHub, INetworkHubReceiver>(_channel, this);

            InitializeHub();

             RegisterDisconnectEvent(_networkHub)
                 .Forget();
        }

        private void InternetConnectionTest()
        {
            //Check internet connection first
            NetworkTest = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    ConnectedToNetwork.Value = InternConnection.IsConnectedToInternet();
                    Thread.Sleep(2000); //wait 2 seconds before checking connectivity.
                }

                //TODO got to find a way to terminate the infinite loop in the task when the game closes
            }, TaskCreationOptions.RunContinuationsAsynchronously);

            NetworkTest
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    Log(LogLevel.Information, "Disposing thread");
                    NetworkTest.Dispose();
                });

            ConnectedToNetwork
                .ObserveEveryValueChanged(v => v.Value)
                .Subscribe(isConnected =>
                {
                    Log(LogLevel.Information, isConnected.ToString());
                    //If false return to main menu
                });
        }

        private void ServerConsoleProcessEvent()
        {
            //Called when the Server Process Exits
            _serverProcess.Exited += (sender, args) => { Debug.Log("Sever has closed"); };

            //Called on when application write to error stream (Error has occured)
            _serverProcess.ErrorDataReceived += (sender, args) => { };

            //Occurs each time an application writes a line standard output stream
            _serverProcess.OutputDataReceived += (sender, args) => { };

            //called when Server application is disposed
            _serverProcess.Disposed += (sender, args) => { };
        }

        private void InitializeServerConsole()
        {
            _serverProcess = Process.Start(@".\..\Tech.Server\bin\Debug\netcoreapp3.1\Tech.Server.exe");

            _serverProcess?.Start();
        }

        private async UniTaskVoid RegisterDisconnectEvent(INetworkHub networkHub)
        {
            try
            {
                await networkHub.WaitForDisconnect();
            }
            catch (Exception e)
            {
                Log(LogLevel.Error, $"Unexpected error on networkHub \n {e.Message}");
            }
            finally
            {
                if (OnServerDisconnect.HasObservers) OnServerDisconnect.OnNext(Unit.Default);

                Log(LogLevel.Information, "Disconnected from Server...");
                //Bring you back to main menu with a disconnection text prompt


                //retry to connect to server
            }
        }

        private async Task<GameObject> InitializeHub()
        {
            var playersRoom = await _networkHub.JoinAsync("bob");
            return null;
        }

        private async UniTaskVoid OnDestroy()
        {
            //terminate server
            await _networkHub.TerminateAsync();

            //client
            await _networkHub.DisposeAsync();
            await _channel.ShutdownAsync();

            _serverProcess.Close();
            _serverProcess.Dispose();
        }

        private void Log(LogLevel levelLevel, string msg)
        {
            if (!verbose) return;
            LogManager.Logger.ZLog(levelLevel, msg);
        }
    }
}