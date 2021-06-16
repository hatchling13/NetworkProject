using AdonisUI.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using NetworkProject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkProject.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {
        public Server ServerInstance { get; private set; }
        public Client ClientInstance { get; private set; }

        public MainViewModel()
        {
            if (Server.Instance.IsUsed)
            {
                ServerInstance = Server.Instance;
            }

            if (Client.Instance.IsUsed)
            {
                ClientInstance = Client.Instance;
            }

            CaptureCommand = new RelayCommand(Capture);
            SendCommand = new RelayCommand(Send);
            CloseCommand = new RelayCommand<AdonisWindow>(Close);
            CleanUpCommand = new RelayCommand<CancelEventArgs>(CleanUp);
        }

        public ICommand CaptureCommand { get; set; }
        private void Capture()
        {
            CaptureDialog dialog = new();
            _ = dialog.ShowDialog();
        }

        public ICommand SendCommand { get; set; }
        private void Send()
        {
            System.Diagnostics.Debug.WriteLine("Send");
            
            if (ServerInstance != null)
            {

            }
            else
            {
                
            }
        }

        public ICommand CloseCommand { get; set; }
        private void Close(AdonisWindow current)
        {
            current.Close();
        }

        public ICommand CleanUpCommand { get; set; }
        public async void CleanUp(CancelEventArgs e)
        {
            if (ServerInstance != null)
            {
                ServerInstance.Stop();
            }

            if (ClientInstance != null)
            {
                e.Cancel = true;

                bool canClose = await ClientInstance.RequestStop();

                if (canClose)
                {
                    ClientInstance.Stop();
                    e.Cancel = false;
                }
            }
        }
    }
}