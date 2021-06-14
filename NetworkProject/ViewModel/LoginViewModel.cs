using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkProject.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            ServerMode = false;
        }

        private bool serverMode;

        public bool ServerMode
        {
            get => serverMode;
            set
            {
                serverMode = value;
                OnPropertyChanged("ServerMode");
            }
        }

        private ICommand startCommand;
        public ICommand StartCommand => startCommand ??= new DelegateCommand(Start);

        private void Start()
        {
            // if (serverMode) server Start
            // else client Start
        }
    }
}
