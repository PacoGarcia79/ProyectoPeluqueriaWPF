using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo que se muestra para confirmación
    /// </summary>
    public class DialogoConfirmacionVM : INotifyPropertyChanged
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyPropertyChanged("Message");
                }
            }
        }

        public DialogoConfirmacionVM(string message)
        {
            Message = message;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
