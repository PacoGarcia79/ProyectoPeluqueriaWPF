using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de la habilitación y deshabilitación de los horarios del establecimiento
    /// para cada uno de los empleados, teniendo en cuenta eventos como bajas, vacaciones o días de cierre.
    /// </summary>
    public class Disponibilidad : INotifyPropertyChanged
    {

        private int _idDisponibilidad;
        public int IdDisponibilidad
        {
            get { return _idDisponibilidad; }
            set
            {
                if (_idDisponibilidad != value)
                {
                    _idDisponibilidad = value;
                    NotifyPropertyChanged("IdDisponibilidad");
                }
            }
        }

        private int _idUsuario;
        public int IdUsuario
        {
            get { return _idUsuario; }
            set
            {
                if (_idUsuario != value)
                {
                    _idUsuario = value;
                    NotifyPropertyChanged("IdUsuario");
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

        private int _idHorario;
        public int IdHorario
        {
            get { return _idHorario; }
            set
            {
                if (_idHorario != value)
                {
                    _idHorario = value;
                    NotifyPropertyChanged("IdHorario");
                }
            }
        }

        private string _hora;
        public string Hora
        {
            get { return _hora; }
            set
            {
                if (_hora != value)
                {
                    _hora = value;
                    NotifyPropertyChanged("Hora");
                }
            }
        }

        private DateTime _fecha_Comienzo;
        public DateTime Fecha_Comienzo
        {
            get { return _fecha_Comienzo; }
            set
            {
                if (_fecha_Comienzo != value)
                {
                    _fecha_Comienzo = value;
                    NotifyPropertyChanged("Fecha_Comienzo");
                }
            }
        }

        private DateTime _fecha_Fin;
        public DateTime Fecha_Fin
        {
            get { return _fecha_Fin; }
            set
            {
                if (_fecha_Fin != value)
                {
                    _fecha_Fin = value;
                    NotifyPropertyChanged("Fecha_Fin");
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }

        public Disponibilidad(int idDisponibilidad, int idUsuario, string nombre, int idHorario, string hora, DateTime fecha_comienzo, DateTime fecha_fin)
        {
            IdDisponibilidad = idDisponibilidad;
            IdUsuario = idUsuario;
            Nombre = nombre;
            IdHorario = idHorario;
            Hora = hora;
            Fecha_Comienzo = fecha_comienzo;
            Fecha_Fin = fecha_fin;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
