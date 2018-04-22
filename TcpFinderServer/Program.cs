using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using TcpFinderServer.Properties;
using System.Linq;
using MultiPlayerLibrary;

namespace TcpFinderServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpPlayerFinderServer server = new TcpPlayerFinderServer(new IPEndPoint(IPAddress.Any, Settings.Default.serverPort));
            server.ServeAsync();
            while (Console.ReadLine() != "exit") ;
            server.Dispose();
        }
    }
    public class TcpPlayerFinderServer : IDisposable
    {
        private TcpListener TcpListener;
        private volatile List<Host> Hosts = new List<Host>();
        private CancellationTokenSource AwaithostToken = new CancellationTokenSource();
        private CancellationTokenSource ServerToken = new CancellationTokenSource();
        private Task HostProcessTask;

        public event Action<Host> OnHostInfoAvailable;
        public event Action<Host> OnHostDelete;

        public TcpPlayerFinderServer(IPEndPoint Point)
        {
            TcpListener = new TcpListener(Point);
            OnHostInfoAvailable += (host) =>
            {
                Console.WriteLine($"Add: {host}");
                Hosts.Add(host);
            };
            OnHostDelete += (host) =>
            {
                Console.WriteLine($"Delete: {host}");
                Hosts.Remove(host);
            };
        }

        public async void ServeAsync()
        {
            TcpListener.Start();
            while (!ServerToken.IsCancellationRequested)
            {
                TcpClient Client = TcpListener.AcceptTcpClient();
                ShowHostsTo(Client);
                Console.WriteLine("Ok");
                HostProcessTask = new Task(ProcessHost, Client, AwaithostToken.Token);
                HostProcessTask.Start();
            }
        }

        private void ShowHostsTo(TcpClient Client)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var stream = Client.GetStream())
            using(BinaryWriter bw=new BinaryWriter(stream))
            {
                bf.Serialize(stream,Hosts);
            }
        }

        private void ProcessHost(object obj)
        {
            TcpClient client = obj as TcpClient;
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            while (!AwaithostToken.IsCancellationRequested )
            {
                if (client.Connected)
                {
                    Console.WriteLine("Process");
                    using (var stream = client.GetStream())
                    using (var br = new BinaryReader(stream))
                    {
                        try
                        {
                            //To avoid many threads
                            stream.ReadTimeout = 10;
                            object data = binaryFormatter.Deserialize(new MemoryStream(br.ReadBytes(1000)));

                            //Add new Host
                            if (data is Host)
                            {
                                Host host = data as Host;
                                host.Creator = client;
                                OnHostInfoAvailable(host);
                            }
                            else
                                //Delete Host Some curved data doesn't matter
                            {
                                OnHostDelete(Hosts.Single(x => x.Creator == client));
                                AwaithostToken.Cancel();
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }

                    }
                }
            }
        }
        public void Dispose()
        {
            TcpListener?.Stop();
            ServerToken?.Cancel();
            AwaithostToken?.Cancel();
        }


    }
}
