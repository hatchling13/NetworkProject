using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NetworkProject.Model
{
    public class Server
    {
        private Server() { }
        private static readonly Lazy<Server> instance = new(() => new Server());
        public static Server Instance => instance.Value;

        private TcpListener listener;
        private List<TcpClient> clients;

        private List<User> permitted;
        public ObservableCollection<User> Users
        {
            get;
            private set;
        }

        public bool IsUsed { get; private set; }

        public bool Init(int port)
        {
            bool result = true;
            
            permitted = new();
            clients = new();

            foreach (int i in Enumerable.Range(0, 10))
            {
                User user = new();
                user.ID = i;
                user.Username = $"user{i + 1}";
                permitted.Add(user);
            }

            try
            {
                IsUsed = true;
                Users = new();

                IPHostEntry entry = Dns.GetHostEntry("");
                IPAddress address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
                listener = new TcpListener(address, port);
                listener.Start();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                result = false;
            }

            return result;
        }

        public async Task Run()
        {
            while (IsUsed)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                    if (clients.Count < 10)
                    {
                        clients.Add(client);
                    }
                    else
                    {
                        throw new ConstraintException("Client count can be up to 9 per Server.");
                    }

                    _ = Task.Factory.StartNew(Read, client);
                }

                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        public void Stop()
        {
            try
            {
                clients.ForEach(c => c.Close());

                listener.Stop();
                IsUsed = false;

                System.Diagnostics.Debug.WriteLine("Server closed");
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        private async void Read(object o)
        {
            TcpClient client = o as TcpClient;
            NetworkStream stream = client.GetStream();

            while (client.Connected)
            {
                try
                {
                    int MAX_SIZE = 2048;
                    var buff = new byte[MAX_SIZE];
                    var bytes = await stream.ReadAsync(buff.AsMemory(0, buff.Length)).ConfigureAwait(false);

                    if (bytes > 0)
                    {
                        ClientProtocol protocol = Serde.Deserialize(buff) as ClientProtocol;
                        Process(client, protocol);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }

        private void Process(TcpClient client, ClientProtocol protocol)
        {
            switch (protocol.Type)
            {
                case ClientType.CONNECT:
                    System.Diagnostics.Debug.WriteLine("Received connect");

                    if (!Connect(protocol.Data))
                    {
                        Disconnect(client, protocol.Data);
                    }

                    break;
                case ClientType.DISCONNECT:
                    System.Diagnostics.Debug.WriteLine("Received disconnect");
                    Disconnect(client, protocol.Data);
                    break;
                case ClientType.CHAT:
                    System.Diagnostics.Debug.WriteLine("Received chat");
                    break;
                case ClientType.IMAGE:
                    System.Diagnostics.Debug.WriteLine("Received image");
                    break;
            }
        }

        private bool Connect(byte[] data)
        {
            bool result = true;

            string name = Encoding.ASCII.GetString(data);

            if (!Users.Any(u => u.Username == name))
            {
                try
                {
                    User user = permitted.Find(u => u.Username == name);
                    System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        Users.Add(user);
                    }));
                }
                catch (ArgumentNullException e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    result = false;
                }
            }

            return result;
        }

        private void Disconnect(TcpClient client, byte[] data)
        {
            string name = Encoding.ASCII.GetString(data);

            try
            {
                User user = Users.First(u => u.Username == name);

                System.Windows.Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    Users.Remove(user);
                }));
                
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            ServerProtocol protocol = new();
            protocol.Type = ServerType.ACCEPT;

            client.GetStream().Write(Serde.Serialize(protocol));

            client.Close();
        }
    }
}
