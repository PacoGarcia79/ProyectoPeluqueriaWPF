using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de citas
    /// </summary>
    public class Cita : INotifyPropertyChanged
    {

        private int _idCliente;
        public int IdCliente
        {
            get { return _idCliente; }
            set
            {
                if (_idCliente != value)
                {
                    _idCliente = value;
                    NotifyPropertyChanged("IdCliente");
                }
            }
        }

        private int _idHorarioEmpleado;
        public int IdHorarioEmpleado
        {
            get { return _idHorarioEmpleado; }
            set
            {
                if (_idHorarioEmpleado != value)
                {
                    _idHorarioEmpleado = value;
                    NotifyPropertyChanged("IdHorarioEmpleado");
                }
            }
        }

        private int _idCita;
        public int IdCita
        {
            get { return _idCita; }
            set
            {
                if (_idCita != value)
                {
                    _idCita = value;
                    NotifyPropertyChanged("IdCita");
                }
            }
        }

        private DateTime _fecha;
        public DateTime Fecha
        {
            get { return _fecha; }
            set
            {
                if (_fecha != value)
                {
                    _fecha = value;
                    NotifyPropertyChanged("Fecha");
                }
            }
        }

        private string _cliente;
        public string Cliente
        {
            get { return _cliente; }
            set
            {
                if (_cliente != value)
                {
                    _cliente = value;
                    NotifyPropertyChanged("Cliente");
                }
            }
        }

        private string _profesional;
        public string Profesional
        {
            get { return _profesional; }
            set
            {
                if (_profesional != value)
                {
                    _profesional = value;
                    NotifyPropertyChanged("Profesional");
                }
            }
        }


        private bool _seleccionada;
        public bool Seleccionada
        {
            get { return _seleccionada; }
            set
            {
                if (_seleccionada != value)
                {
                    _seleccionada = value;
                    NotifyPropertyChanged("Seleccionada");
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

        private string _servicios;
        public string Servicios
        {
            get { return _servicios; }
            set
            {
                if (_servicios != value)
                {
                    _servicios = value;
                    NotifyPropertyChanged("Servicios");
                }
            }
        }

        private double _precio_Servicios;
        public double Precio_Servicios
        {
            get { return _precio_Servicios; }
            set
            {
                if (_precio_Servicios != value)
                {
                    _precio_Servicios = value;
                    NotifyPropertyChanged("Precio_Servicios");
                }
            }
        }

        private string _productos;
        public string Productos
        {
            get { return _productos; }
            set
            {
                if (_productos != value)
                {
                    _productos = value;
                    NotifyPropertyChanged("Productos");
                }
            }
        }

        private string _cantidad;
        public string Cantidad
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

        private double _precio_Productos;
        public double Precio_Productos
        {
            get { return _precio_Productos; }
            set
            {
                if (_precio_Productos != value)
                {
                    _precio_Productos = value;
                    NotifyPropertyChanged("Precio_Productos");
                }
            }
        }

        private bool _cancelada;
        public bool Cancelada
        {
            get { return _cancelada; }
            set
            {
                if (_cancelada != value)
                {
                    _cancelada = value;
                    NotifyPropertyChanged("Cancelada");
                }
            }
        }

        public Cita()
        {
        }

        public Cita(int idCita)
        {
            IdCita = idCita;
        }

        public Cita(int idCliente, int idHorarioEmpleado, DateTime fecha, bool cancelada)
        {
            IdCliente = idCliente;
            IdHorarioEmpleado = idHorarioEmpleado;
            Fecha = fecha;
            Cancelada = cancelada;
        }

        public Cita(int idCita, DateTime fecha, string cliente, string profesional, string hora, string servicios, 
            double precio_Servicios, string productos, string cantidad, double precio_Productos)
        {
            IdCita = idCita;
            Fecha = fecha;
            Cliente = cliente;
            Profesional = profesional;
            Hora = hora;
            Servicios = servicios;
            Precio_Servicios = precio_Servicios;
            Productos = productos;
            Cantidad = cantidad;
            Precio_Productos = precio_Productos;
        }

        public Cita(int idCita, DateTime fecha, string cliente, string profesional, string hora, string servicios,
            double precio_Servicios, string productos, double precio_Productos)
        {
            IdCita = idCita;
            Fecha = fecha;
            Cliente = cliente;
            Profesional = profesional;
            Hora = hora;
            Servicios = servicios;
            Precio_Servicios = precio_Servicios;
            Productos = productos;
            Precio_Productos = precio_Productos;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
