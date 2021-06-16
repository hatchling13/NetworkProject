using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetworkProject.Model
{
    public class Client
    {
        private Client() { }
        private static readonly Lazy<Client> instance = new(() => new Client());
        public static Client Instance => instance.Value;

        private TcpClient client;
        private string userName;

        public bool IsUsed { get; private set; }

        public bool Init(string address, int port, string name)
        {
            bool result = true;

            try
            {
                IsUsed = true;
                userName = name;
                client = new TcpClient(address, port);

                ClientProtocol first = new();
                first.Type = ClientType.CONNECT;
                first.Data = Encoding.ASCII.GetBytes(userName);

                byte[] buff = Serde.Serialize(first);

                NetworkStream stream = client.GetStream();
                stream.Write(buff.AsMemory(0, buff.Length).Span);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                result = false;
            }

            return result;
        }

        public void Stop()
        {
            client.Close();
            System.Diagnostics.Debug.WriteLine("Client closed");
        }

        public async Task<bool> RequestStop()
        {
            bool result = true;
            
            ClientProtocol last = new();
            last.Type = ClientType.DISCONNECT;
            last.Data = Encoding.ASCII.GetBytes(userName);

            try
            {
                NetworkStream stream = client.GetStream();

                byte[] buff = Serde.Serialize(last);
                await stream.WriteAsync(buff.AsMemory(0, buff.Length));
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                result = false;
            }

            return result;
        }
    }
}