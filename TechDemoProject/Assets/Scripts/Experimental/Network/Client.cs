using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

namespace Experimental.Network
{
    public class Client : MonoBehaviour
    {
        public static Client instance;
        public static int dataBufferSize = 4096;

        public string ip = "127.0.0.1";
        public int port = 26950;

        private delegate void PacketHandler(Packet _packet);

        private static Dictionary<int, PacketHandler> _packetHandlers;
        
        public int myId = 0;
        public TCP tcp;

        public UDP udp;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }


        private void Start()
        {
            tcp = new TCP();
            udp = new UDP();
        }


        public void ConnectToServer()
        {
            InitializeClientData();
            tcp.Connect();
        }

        public class TCP
        {
            public TcpClient socket;
            private NetworkStream _stream;
            private Packet recievedData;
            private byte[] _receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };


                _receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);

            }

            
            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                _stream = socket.GetStream();
                
                recievedData = new Packet();
                

                _stream.BeginRead(_receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = _stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        // Disconnect
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(_receiveBuffer, _data, _byteLength);

                    recievedData.Reset(HandleData(_data));
                    
                    _stream.BeginRead(_receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);


                }
                catch
                {

                }
            }

            private bool HandleData(byte[] data)
            {
                int _packetLength = 0;
                
                recievedData.SetBytes(data);

                if (recievedData.UnreadLength() >= 4)
                {
                    _packetLength = recievedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= recievedData.UnreadLength())
                {
                    byte[] _packetBytes = recievedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            int _packetId = _packet.ReadInt();
                            _packetHandlers[_packetId](_packet);
                        }
                    });
                    
                    _packetLength = 0;
                    if (recievedData.UnreadLength() >= 4)
                    {
                        _packetLength = recievedData.ReadInt();
                        if (_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }
                if(_packetLength <= 1)
                    return true;

                return false;
            }

            public void SendData(Packet packet)
            {
                try
                {
                    if (socket != null)
                    {
                        _stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        
        public class UDP
        {
            public UdpClient socket;
            public IPEndPoint EndPoint;

            public UDP()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(instance.ip),instance.port);
            }

            public void Connect(int lockPort)
            {
                socket = new UdpClient(lockPort);
                
                socket.Connect(EndPoint);
                socket.BeginReceive(ReceiveCallback, null);

                using (Packet packet = new Packet())
                {
                    SendData(packet);
                }
            }

            public void SendData(Packet packet)
            {
                try
                {
                    packet.InsertInt(instance.myId);
                    if (socket != null)
                    {
                        socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error sending data to server via UDP: {e}");
                    throw;
                }
            }
            
            private void ReceiveCallback(IAsyncResult result)
            {
                try
                {
                    byte[] _data = socket.EndReceive(result, ref EndPoint);
                    socket.BeginReceive(ReceiveCallback, null);

                    if (_data.Length < 4)
                    {
                        // disconnect
                        return;
                    }

                    HandleData(_data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;    
                }
            }

            private void HandleData(byte[] data)
            {
                using (Packet packet = new Packet(data))
                {
                    int _packetLength = packet.ReadInt();
                    data = packet.ReadBytes(_packetLength);
                }
                
                ThreadManager.ExecuteOnMainThread((() =>
                {
                    using (Packet packet = new Packet(data))
                    {
                        int packetId = packet.ReadInt();
                        _packetHandlers[packetId](packet);
                    }
                }));
            }
        }
        
        private void InitializeClientData()
        {
            _packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome},  
                {(int)ServerPackets.SpawnPlayer, ClientHandle.SpawnPlayer}
            };
            Debug.Log("Initialized packets");
        }
    }
}