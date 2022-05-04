using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private const string DESCRIPCION = "Proin dignissim eleifend urna, at blandit ante ullamcorper in. Fusce laoreet nunc ac viverra " +
            "tincidunt. Duis porttitor efficitur tristique. Sed pharetra ipsum libero, in dictum dolor accumsan eget. Sed in felis nulla. " +
            "Sed pretium, est id convallis placerat, nisl est eleifend dolor, sit amet iaculis lectus sem quis ligula. Praesent a pretium est. " +
            "Etiam sit amet quam non leo venenatis egestas. Praesent sit amet sagittis arcu, ac sagittis est. Duis mollis arcu id dapibus vestibulum. " +
            "Donec tristique, turpis sit amet ultricies sagittis, velit ex dictum ex, eu pulvinar tellus nisi non urna. Maecenas cursus dolor sed " +
            "lacus facilisis vulputate. Pellentesque tincidunt ornare efficitur. Phasellus lectus lorem, mattis eu mauris ut, consequat ultricies " +
            "neque. Ut aliquet ipsum et ullamcorper molestie.";

        private const string FOTO = "/Img/peluqueria.jpg";

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
            Texto = DESCRIPCION;
            Imagen = FOTO;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
