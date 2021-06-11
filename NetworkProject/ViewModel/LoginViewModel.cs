using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkProject.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            ProgramMode = false;
        }
        
        private bool programMode;

        public bool ProgramMode
        {
            get => programMode;
            set
            {
                programMode = value;
                OnPropertyChanged("ProgramMode");
            }
        }
    }
}
