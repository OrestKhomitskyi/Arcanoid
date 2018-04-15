using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Arcanoid.Multiplayer
{
    public class TcpPlayerFinderServer : IDisposable
    {
        private TcpListener TcpListener;
        private bool IsListening = true;
        private volatile List<TcpClient> Clients = new List<TcpClient>();

        public TcpPlayerFinderServer(IPEndPoint Point)
        {
            TcpListener = new TcpListener(Point);
        }

        public async void ServeAsync()
        {
            while (true)
            {
                TcpClient Client = await TcpListener.AcceptTcpClientAsync();
                Clients.Add(Client);

            }
        }

        public void BroadCastMessage()
        {

        }

        public void Dispose()
        {

            TcpListener?.Stop();
        }


    }
}
