using AdonisUI.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkProject.ViewModel
{
    public class MainViewModel : ObservableRecipient
    {
        public MainViewModel()
        {
            CaptureCommand = new RelayCommand(Capture);
            CloseCommand = new RelayCommand<AdonisWindow>(Close);
        }

        public ICommand CaptureCommand { get; set; }
        private void Capture(object o)
        {
            CaptureDialog dialog = new();
            dialog.ShowDialog();
        }

        public ICommand CloseCommand { get; set; }
        private void Close(AdonisWindow current)
        {
            current.Close();
        }
    }
}