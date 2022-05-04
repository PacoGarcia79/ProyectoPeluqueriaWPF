using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de los productos
    /// </summary>
    public class Producto : INotifyPropertyChanged
    {
        private int _idProducto;
        public int IdProducto
        {
            get { return _idProducto; }
            set
            {
                if (_idProducto != value)
                {
                    _idProducto = value;
                    NotifyPropertyChanged("IdProducto");
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

        private int _idProductoGrupo;
        public int IdProductoGrupo
        {
            get { return _idProductoGrupo; }
            set
            {
                if (_idProductoGrupo != value)
                {
                    _idProductoGrupo = value;
                    NotifyPropertyChanged("IdProductoGrupo");
                }
            }
        }

        private string _nombreGrupo;
        public string NombreGrupo
        {
            get { return _nombreGrupo; }
            set
            {
                if (_nombreGrupo != value)
                {
                    _nombreGrupo = value;
                    NotifyPropertyChanged("Nombregrupo");
                }
            }
        }

        private string _descripcion;
        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                if (_descripcion != value)
                {
                    _descripcion = value;
                    NotifyPropertyChanged("Descripcion");
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


        private bool _añadeCita;
        public bool AñadeCita
        {
            get { return _añadeCita; }
            set
            {
                if (_añadeCita != value)
                {
                    _añadeCita = value;
                    NotifyPropertyChanged("AñadeCita");
                }
            }
        }

        private int _stock;
        public int Stock
        {
            get { return _stock; }
            set
            {
                if (_stock != value)
                {
                    _stock = value;
                    NotifyPropertyChanged("Stock");
                }
            }
        }

        private int _cantidad;
        public int Cantidad
        {
            get { return _cantidad; }
            set
            {
                if (_cantidad != value)
                {
                    _cantidad = value;
                    NotifyPropertyChanged("Cantidad");
                }
            }
        }

        /// <summary>
        /// Ruta de la foto del producto que se va a modificar
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


        public Producto()
        {
            IdProducto = 0;
            Nombre = "";
            Precio = 0.0;
            IdProductoGrupo = 0;
            NombreGrupo = "";
            Descripcion = "";
            Foto = "";
        }

        public Producto(string nombre, double? precio, int idProductoGrupo, string descripcion, string foto)
        {
            Nombre = nombre;
            Precio = precio;
            IdProductoGrupo = idProductoGrupo;
            Descripcion = descripcion;
            Foto = foto;
        }


        public Producto(int idProducto, string nombre, double? precio, int idProductoGrupo, string nombreGrupo, string descripcion, string foto, int stock)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Precio = precio;
            IdProductoGrupo = idProductoGrupo;
            NombreGrupo = nombreGrupo;
            Descripcion = descripcion;
            Foto = foto;
            Stock = stock;
        }

        public Producto(int idProducto, string nombre, double? precio, int idProductoGrupo, string descripcion, string foto)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Precio = precio;
            IdProductoGrupo = idProductoGrupo;
            Descripcion = descripcion;
            Foto = foto;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
