using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Arcanoid.Multiplayer
{
    public class TcpPlayerClient : IDisposable
    {
        private TcpClient TcpClient;

        public event Action<Player> OnInfoReceived;
        public event Action OnConnected;

        public IPEndPoint ServerEndPoint { get; set; }


        public TcpPlayerClient(IPEndPoint local, IPEndPoint remote)
        {
            TcpClient = new TcpClient(local);
            ServerEndPoint = remote;
        }

        public async void Connect()
        {
            await TcpClient.ConnectAsync(ServerEndPoint.Address, ServerEndPoint.Port);
        }

        public async void GetMessages()
        {
            using (NetworkStream networkStream = TcpClient.GetStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                Player playerInfo = (Player)bf.Deserialize(networkStream);
                OnInfoReceived(playerInfo);
            }
        }


        public void Dispose()
        {
            TcpClient.Close();
        }
    }
}
