using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de los diferentes grupos en los que se engloban los productos
    /// </summary>
    public class ProductoGrupo : INotifyPropertyChanged
    {
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
                    NotifyPropertyChanged("NombreGrupo");
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

      
        public ProductoGrupo()
        {
            IdProductoGrupo= 0;
            NombreGrupo = "";
            Foto = "";
        }

        public ProductoGrupo(int idProductoGrupo, string nombreGrupo, string foto)
        {
            IdProductoGrupo = idProductoGrupo;
            NombreGrupo = nombreGrupo;
            Foto = foto;
        }

        public ProductoGrupo(string nombreGrupo, string foto)
        {
            NombreGrupo = nombreGrupo;
            Foto = foto;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
