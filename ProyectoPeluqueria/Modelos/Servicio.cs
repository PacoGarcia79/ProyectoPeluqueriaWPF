using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de los servicios que ofrece el establecimiento
    /// </summary>
    public class Servicio : INotifyPropertyChanged
    {       

        private int _idServicio;
        public int IdServicio
        {
            get { return _idServicio; }
            set
            {
                if (_idServicio != value)
                {
                    _idServicio = value;
                    NotifyPropertyChanged("IdServicio");
                }
            }
        }


        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    NotifyPropertyChanged("Nombre");
                }
            }
        }


        private double? _precio;
        public double? Precio
        {
            get { return _precio; }
            set
            {
                if (_precio != value)
                {
                    _precio = value;
                    NotifyPropertyChanged("Precio");
                }
            }
        }

        private string _foto;
        public string Foto
        {
            get { return _foto; }
            set
            {
                if (_foto != value)
                {
                    _foto = value;
                    NotifyPropertyChanged("Foto");
                }
            }
        }

        private bool _seleccionado;
        public bool Seleccionado
        {
            get { return _seleccionado; }
            set
            {
                if (_seleccionado != value)
                {
                    _seleccionado = value;
                    NotifyPropertyChanged("Seleccionado");
                }
            }
        }

        /// <summary>
        /// Ruta de la foto del servicio que se va a modificar
        /// </summary>
        private string _rutaFotoNueva;
        public string RutaFotoNueva
        {
            get { return _rutaFotoNueva; }
            set
            {
                if (_rutaFotoNueva != value)
                {
                    _rutaFotoNueva = value;
                    NotifyPropertyChanged("RutaFotoNueva");
                }
            }
        }


        public Servicio()
        {
            IdServicio = 0;
            Nombre = "";
            Precio = 0.0;
            Foto = "";
        }

        public Servicio(int idServicio, string nombre, double? precio, string foto)
        {
            IdServicio = idServicio;
            Nombre = nombre;
            Precio = precio;
            Foto = foto;
        }

        public Servicio(string nombre, double? precio, string foto)
        {
            Nombre = nombre;
            Precio = precio;
            Foto = foto;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{IdServicio}";
        }
    }
}
