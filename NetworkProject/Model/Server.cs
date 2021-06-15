using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkProject.Model
{
    public class Server
    {
        private Server() { }
        private static readonly Lazy<Server> instance = new(() => new Server());
        public static Server Instance
        {
            get => instance.Value;
        }

        private TcpListener listener;

        public bool Init(int port)
        {
            bool result = true;

            try
            {
                IPHostEntry entry = Dns.GetHostEntry("");
                IPAddress address = entry.AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
                listener = new TcpListener(address, port);
                listener.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }

            return result;
        }

        public async void Run()
        {
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);
                byte[] data = new byte[2048];
                client.GetStream().Read(data);

                string result = Encoding.Default.GetString(data);
                System.Diagnostics.Debug.WriteLine(result);
            }
        }
    }
}
