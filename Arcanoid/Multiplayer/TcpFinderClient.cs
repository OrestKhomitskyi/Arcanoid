using Arcanoid.Properties;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MultiPlayerLibrary;


namespace Arcanoid.Multiplayer
{
    public class TcpFinderClient : IDisposable
    {
        private TcpClient TcpClient;
        private IPEndPoint ServerEndPoint { get; set; }
        private Task ProcessMessages;
        private CancellationTokenSource ProcessMessageToken=new CancellationTokenSource();

        public TcpFinderClient(IPEndPoint local)
        {
            TcpClient = new TcpClient(local);
            ServerEndPoint = new IPEndPoint(IPAddress.Any,Settings.Default.serverPort);
            TcpClient.Connect(ServerEndPoint.Address, ServerEndPoint.Port);
        }
        
        public Task GetMessagesAsync()
        {
            return Task.Run(() =>
            {
                BinaryFormatter bf = new BinaryFormatter();
                while (!ProcessMessageToken.IsCancellationRequested)
                {
                    if(TcpClient.Connected)
                        using (NetworkStream networkStream = TcpClient.GetStream())
                        {
                            object data = bf.Deserialize(networkStream);
                        }
                }
            },ProcessMessageToken.Token);
            
        }

        public void CreateHost()
        {
            if(!TcpClient.Connected)
            {
                throw new Exception("Client Not connected");
            }
            using (var stream=TcpClient.GetStream())
            using(var sw=new StreamWriter(stream,Encoding.ASCII))
            {
                sw.Write(MultiPlayerSignals.CREATE_HOST);
            }
        }
        public void DeleteHost()
        {
            if (!TcpClient.Connected)
            {
                throw new Exception("Client Not connected");
            }
            using (var stream = TcpClient.GetStream())
            using (var sw = new StreamWriter(stream, Encoding.ASCII))
            {
                sw.Write(MultiPlayerSignals.REMOVE_HOST);
            }
        }



        public void Dispose()
        {
            TcpClient?.Close();
        }
    }
}
