using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NetworkProject.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ICommand captureCommand;
        public ICommand CaptureCommand => captureCommand ??= new DelegateCommand(Capture);

        private void Capture()
        {
            CaptureDialog dialog = new();
            _ = dialog.ShowDialog();
        }
    }
}
