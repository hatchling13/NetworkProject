using AdonisUI.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NetworkProject.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace NetworkProject.ViewModel
{
    public class LoginViewModel : ObservableRecipient
    {
        private Server server;
        private Client client;

        public LoginViewModel()
        {
            StartCommand = new RelayCommand<AdonisWindow>(Start);
        }

        private bool serverMode;
        public bool ServerMode
        {
            get => serverMode;
            set => SetProperty(ref serverMode, value);
        }

        private string userName;
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string address;
        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        private string sendPort;
        public string SendPort
        {
            get => sendPort;
            set => SetProperty(ref sendPort, value);
        }

        private string recvPort;
        public string RecvPort
        {
            get => recvPort;
            set => SetProperty(ref recvPort, value);
        }

        public ICommand StartCommand { get; set; }
        private void Start(AdonisWindow current)
        {
            bool result = true;

            try
            {
                if (serverMode)
                {
                    server = Server.Instance;
                    result = server.Init(int.Parse(RecvPort));
                    
                    if (result)
                    {
                        _ = server.Run();
                    }
                }
                else
                {
                    client = Client.Instance;
                    result = client.Init(Address, int.Parse(SendPort), UserName);
                }
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                result = false;
            }
            finally
            {
                if (result)
                {
                    MainWindow main = new();
                    System.Windows.Application.Current.MainWindow = main;

                    if (serverMode)
                    {
                        main.Title += "(Server)";
                    }
                    else
                    {
                        main.Title += $"({UserName})";
                    }

                    main.Show();
                    current.Close();
                }
            }
        }
    }
}
