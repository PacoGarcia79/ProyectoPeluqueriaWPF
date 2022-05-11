using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Inicio
    /// </summary>
    class UserControlInicioVM : INotifyPropertyChanged
    {
        private string _texto;
        public string Texto
        {
            get { return _texto; }
            set
            {
                if (_texto != value)
                {
                    _texto = value;
                    NotifyPropertyChanged("Texto");
                }
            }
        }

        private string _imagen;
        public string Imagen
        {
            get { return _imagen; }
            set
            {
                if (_imagen != value)
                {
                    _imagen = value;
                    NotifyPropertyChanged("Imagen");
                }
            }
        }

        public UserControlInicioVM()
        {
            Texto = GetDescription();
            Imagen = Properties.Settings.Default.imagen;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string GetDescription()
        {
            try
            {
                string appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string[] paths = { appPath, "descripcion.txt" };

                return File.ReadAllText(Path.Combine(paths));
            }
            catch (IOException)
            {
                return "";
            }
        }
    }
}
