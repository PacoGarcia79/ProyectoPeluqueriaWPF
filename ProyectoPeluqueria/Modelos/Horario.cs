using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de los horarios del establecimiento
    /// </summary>
    public class Horario : INotifyPropertyChanged
    {
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

        public Horario()
        {
        }

        public Horario(int idHorario, string hora)
        {
            IdHorario = idHorario;
            Hora = hora;
        }

        public override string ToString()
        {
            return $"{IdHorario}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
