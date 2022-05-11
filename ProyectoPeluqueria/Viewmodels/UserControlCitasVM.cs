using GalaSoft.MvvmLight.CommandWpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using ProyectoPeluqueria.UserControlMenu.ComandosUserControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Citas
    /// </summary>
    class UserControlCitasVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Boolean para control del refresco de los elementos de las pantallas de opciones de reserva de citas
        /// </summary>
        private bool CompruebaFecha { get; set; }

        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlCitasRootDialogHost";

        /// <summary>
        /// Listado de fechas no disponibles para realizar citas
        /// </summary>
        private ObservableCollection<DateTime> _blackOutDates;
        public ObservableCollection<DateTime> BlackOutDates
        {
            get { return _blackOutDates; }
            set
            {
                if (_blackOutDates != value)
                {
                    _blackOutDates = value;
                    NotifyPropertyChanged("BlackOutDates");
                }
            }
        }

        /// <summary>
        /// Opciones de cita. Al cambiar de opción se obtiene el primer día disponible para confirmar citas
        /// </summary>
        private Opciones _options;
        public Opciones Options
        {
            get { return _options; }
            set
            {
                if (_options != value)
                {
                    _options = value;
                    NotifyPropertyChanged("Options");

                    CompruebaDiasAgenda();
                }
            }
        }

        /// <summary>
        /// Lista de todos lo clientes
        /// </summary>
        private ObservableCollection<Usuario> _listaClientes;
        public ObservableCollection<Usuario> ListaClientes
        {
            get { return _listaClientes; }
            set
            {
                if (_listaClientes != value)
                {
                    _listaClientes = value;
                    NotifyPropertyChanged("ListaClientes");
                }
            }
        }

        /// <summary>
        /// Cliente seleccionado para la confirmación de la cita
        /// </summary>
        private Usuario _clienteSeleccionado;
        public Usuario ClienteSeleccionado
        {
            get { return _clienteSeleccionado; }
            set
            {
                if (_clienteSeleccionado != value)
                {
                    _clienteSeleccionado = value;
                    NotifyPropertyChanged("ClienteSeleccionado");

                }
            }
        }

        /// <summary>
        /// Fecha seleccionada para la confirmación de la cita
        /// </summary>
        private DateTime _fechaSeleccionada;
        public DateTime FechaSeleccionada
        {
            get { return _fechaSeleccionada; }
            set
            {
                _fechaSeleccionada = value;
                NotifyPropertyChanged("FechaSeleccionada");

                if (CompruebaFecha)
                {
                    RefrescaSegunOpcion();
                }

            }
        }

        /// <summary>
        /// Fecha que se muestra en el calendario
        /// </summary>
        private DateTime _fechaActual;
        public DateTime FechaActual
        {
            get { return _fechaActual; }
            set
            {
                _fechaActual = value;
                NotifyPropertyChanged("FechaActual");
            }
        }

        /// <summary>
        /// Fecha fin del intervalo que se mostrará para confirmar citas
        /// </summary>
        private DateTime _fechaFinIntervalo;
        public DateTime FechaFinIntervalo
        {
            get { return _fechaFinIntervalo; }
            set
            {
                if (_fechaFinIntervalo != value)
                {
                    _fechaFinIntervalo = value;
                    NotifyPropertyChanged("FechaFinIntervalo");
                }
            }
        }

        /// <summary>
        /// Lista servicios para confirmar citas
        /// </summary>
        private ObservableCollection<Servicio> _listaServicios;
        public ObservableCollection<Servicio> ListaServicios
        {
            get { return _listaServicios; }
            set
            {
                if (_listaServicios != value)
                {
                    _listaServicios = value;
                    NotifyPropertyChanged("ListaServicios");
                }
            }
        }

        /// <summary>
        /// Listado de los servicios seleccionados para la cita
        /// </summary>
        private IList _listaServiciosSeleccionados;
        public IList ListaServiciosSeleccionados
        {
            get { return _listaServiciosSeleccionados; }
            set
            {
                if (_listaServiciosSeleccionados != value)
                {
                    _listaServiciosSeleccionados = value;
                    NotifyPropertyChanged("ListaServiciosSeleccionados");
                }
            }
        }

        /// <summary>
        /// Servicio seleccionado para la cita
        /// </summary>
        private Servicio _servicioSeleccionado;
        public Servicio ServicioSeleccionado
        {
            get { return _servicioSeleccionado; }
            set
            {
                if (_servicioSeleccionado != value)
                {
                    _servicioSeleccionado = value;
                    NotifyPropertyChanged("ServicioSeleccionado");
                }
            }
        }

        /// <summary>
        /// Lista horarios para confirmar citas
        /// </summary>
        private ObservableCollection<Horario> _listaHorarios;
        public ObservableCollection<Horario> ListaHorarios
        {
            get { return _listaHorarios; }
            set
            {
                if (_listaHorarios != value)
                {
                    _listaHorarios = value;
                    NotifyPropertyChanged("ListaHorarios");
                }
            }
        }

        /// <summary>
        /// Horario seleccionado para la cita
        /// </summary>
        private Horario _horarioSeleccionado;
        public Horario HorarioSeleccionado
        {
            get { return _horarioSeleccionado; }
            set
            {
                if (_horarioSeleccionado != value)
                {
                    _horarioSeleccionado = value;
                    NotifyPropertyChanged("HorarioSeleccionado");

                    if (Options == Opciones.Hora)
                    {
                        RefrescaListaEmpleados();
                        ListaServicios = new ObservableCollection<Servicio>();
                    }

                }
            }
        }

        /// <summary>
        /// Listado de empleados para la opción de cita por hora
        /// </summary>
        private ObservableCollection<Usuario> _listaEmpleadosCitaPorHora;
        public ObservableCollection<Usuario> ListaEmpleadosCitaPorHora
        {
            get { return _listaEmpleadosCitaPorHora; }
            set
            {
                if (_listaEmpleadosCitaPorHora != value)
                {
                    _listaEmpleadosCitaPorHora = value;
                    NotifyPropertyChanged("ListaEmpleadosCitaPorHora");
                }
            }
        }

        /// <summary>
        /// Listado de empleados para la opción de cita por profesional
        /// </summary>
        private ObservableCollection<Usuario> _listaEmpleadosCitaPorProfesional;
        public ObservableCollection<Usuario> ListaEmpleadosCitaPorProfesional
        {
            get { return _listaEmpleadosCitaPorProfesional; }
            set
            {
                if (_listaEmpleadosCitaPorProfesional != value)
                {
                    _listaEmpleadosCitaPorProfesional = value;
                    NotifyPropertyChanged("ListaEmpleadosCitaPorProfesional");
                }
            }
        }

        /// <summary>
        /// Empleado seleccionado para la cita
        /// </summary>
        private Usuario _empleadoSeleccionado;
        public Usuario EmpleadoSeleccionado
        {
            get { return _empleadoSeleccionado; }
            set
            {
                if (_empleadoSeleccionado != value)
                {
                    _empleadoSeleccionado = value;
                    NotifyPropertyChanged("EmpleadoSeleccionado");

                    if (Options == Opciones.Hora)
                    {
                        RefrescaListaServicios();
                    }
                    else
                    {
                        RefrescaListaServicios();
                        RefrescaListaHorarios();
                    }
                }
            }
        }

        /// <summary>
        /// Refresca las listas según la opción de cita seleccionada
        /// </summary>
        private void RefrescaSegunOpcion()
        {
            if (Options == Opciones.Hora)
            {
                ListaHorarios = ServicioApiRest.GetHorariosLibresFecha(FechaSeleccionada);
                ListaServicios = new ObservableCollection<Servicio>();
                ListaEmpleadosCitaPorHora = new ObservableCollection<Usuario>();
            }
            else
            {
                ListaEmpleadosCitaPorProfesional = ServicioApiRest.GetEmpleadosDisponiblesOpcionProfesional(FechaSeleccionada);
                ListaServicios = new ObservableCollection<Servicio>();
                ListaHorarios = new ObservableCollection<Horario>();
            }
        }

        /// <summary>
        /// Obtiene la lista de empleados disponibles en la opción de citas por hora, o la vacía, según si hay un horario seleccionado
        /// </summary>
        public void RefrescaListaEmpleados()
        {
            if (HorarioSeleccionado != null)
                ListaEmpleadosCitaPorHora = ServicioApiRest.GetEmpleadosDisponiblesOpcionHora(HorarioSeleccionado.IdHorario, FechaSeleccionada);
            else
                ListaEmpleadosCitaPorHora = new ObservableCollection<Usuario>();
        }

        /// <summary>
        /// Obtiene la lista de servicios disponibles, o la vacía, según si hay un empleado seleccionado
        /// </summary
        public void RefrescaListaServicios()
        {
            if (EmpleadoSeleccionado != null)
                ListaServicios = ServicioApiRest.GetServiciosPorEmpleado(EmpleadoSeleccionado.IdUsuario);
            else
                ListaServicios = new ObservableCollection<Servicio>();
        }

        /// <summary>
        /// Obtiene la lista de horarios disponibles por empleado y fecha, o la vacía, según si hay un empleado seleccionado
        /// </summary
        public void RefrescaListaHorarios()
        {
            if (EmpleadoSeleccionado != null)
                ListaHorarios = ServicioApiRest.GetHorariosLibresEmpleadosFecha(EmpleadoSeleccionado.IdUsuario, FechaSeleccionada);
            else
                ListaHorarios = new ObservableCollection<Horario>();
        }

        /// <summary>
        /// Método CanExecute del command para añadir una cita
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => EmpleadoSeleccionado != null && HorarioSeleccionado != null && ClienteSeleccionado != null
            && ServicioSeleccionado != null && ListaServiciosSeleccionados.Count > 0;

        /// <summary>
        /// Obtiene string con los ids de los servicios seleccionados separados por coma a partir del listado
        /// </summary>
        /// <returns>EmpleadosString</returns>
        public string ObtieneListadoServicios()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ListaServiciosSeleccionados.Count; i++)
            {
                if (i < ListaServiciosSeleccionados.Count - 1)
                    sb.Append(ListaServiciosSeleccionados[i].ToString()).Append(",");
                else
                    sb.Append(ListaServiciosSeleccionados[i].ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Método Execute del command para añadir una cita. Añade la cita y comprueba de nuevo el primer día disponible.
        /// </summary>        
        public void OnAdd()
        {
            string servicios = ObtieneListadoServicios();

            Response = ServicioApiRest.PostCita(HorarioSeleccionado.IdHorario, EmpleadoSeleccionado.IdUsuario,
                FechaSeleccionada, ClienteSeleccionado.IdUsuario, servicios);

            if (Response.Mensaje == "Registro insertado")
            {
                MuestraDialogo("Cita añadida");

                LimpiaListasYObjetos();

                CompruebaFecha = false;
                FechaFinIntervalo = DateTime.Now.AddMonths(4);
                FechaSeleccionada = DateTime.Now.AddMonths(3);

                CompruebaFecha = true;
                CompruebaDiasAgenda();


            }
            else
            {
                MuestraDialogo("Ha habido un error al añadir la cita");
            }
        }

        /// <summary>
        /// Limpia listas y objectos seleccionados después de añadir una nueva cita
        /// </summary>
        private void LimpiaListasYObjetos()
        {
            HorarioSeleccionado = null;
            ServicioSeleccionado = null;
            EmpleadoSeleccionado = null;
            ClienteSeleccionado = null;
            ListaServicios = new ObservableCollection<Servicio>();
            ListaServiciosSeleccionados = new ObservableCollection<Servicio>();
            _listaServiciosSeleccionados = new ArrayList();
            ListaHorarios = new ObservableCollection<Horario>();
            ListaEmpleadosCitaPorHora = new ObservableCollection<Usuario>();
        }

        /// <summary>
        /// Obtiene el primer día disponible para confirmar citas, que se mostrará en el calendario
        /// </summary>
        public void CompruebaDiasAgenda()
        {
            BlackOutDates = ServicioApiRest.GetFechasOcupadas(DateTime.Now);

            DateTime closestDate = new DateTime();
            int contador = 0;
            DateTime fechaComienzoBusqueda;

            if (BlackOutDates[0].ToShortDateString() != DateTime.Now.ToShortDateString())
            {
                FechaActual = DateTime.Now;

                FechaSeleccionada = DateTime.Now;

                FechaFinIntervalo = DateTime.Now.AddDays(60);
            }
            else
            {
                fechaComienzoBusqueda = DateTime.Now;
                for (var dt = fechaComienzoBusqueda; dt <= DateTime.Now.AddDays(60); dt = dt.AddDays(1))
                {
                    if (BlackOutDates[contador].ToShortDateString() != dt.ToShortDateString())
                    {
                        closestDate = dt;
                        break;
                    }

                    contador++;
                }

                FechaSeleccionada = closestDate;

                FechaActual = closestDate;

                FechaFinIntervalo = closestDate.AddDays(60);
            }
        }

        /// <summary>
        /// Muestra un diálogo informativo con un mensaje pasado por parámetro
        /// </summary>
        /// <param name="mensaje">string a mostrar</param>
        public void MuestraDialogo(string mensaje)
        {
            Dialogo dialogo = new Dialogo();
            dialogo.MensajeText.Text = mensaje;
            dialogo.ShowDialog();
        }

        public UserControlCitasVM()
        {
            CompruebaFecha = true;

            _listaServiciosSeleccionados = new ArrayList();
            ListaClientes = ServicioApiRest.GetClientes();
            ListaServicios = new ObservableCollection<Servicio>();
            ListaServiciosSeleccionados = new ObservableCollection<Servicio>();
            ListaEmpleadosCitaPorHora = new ObservableCollection<Usuario>();
            Response = new MensajeGeneral();
            ListaEmpleadosCitaPorProfesional = new ObservableCollection<Usuario>();

            Options = Opciones.Hora;

            CompruebaDiasAgenda();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}