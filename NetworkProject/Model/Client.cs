using AdonisUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkProject.Model
{
    public class Client
    {
        private Client() { }
        private static readonly Lazy<Client> instance = new(() => new Client());
        public static Client Instance
        {
            get => instance.Value;
        }

        private TcpClient client;
        private string userName;

        public bool Init(string address, int port, string name)
        {
            bool result = true;

            try
            {
                userName = name;
                client = new TcpClient(address, port);

                byte[] data = System.Text.Encoding.ASCII.GetBytes(userName);
                client.GetStream().Write(data);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine(e.Message);
                result = false;
            }

            return result;
        }

        public void Run()
        {

        }
    }
}