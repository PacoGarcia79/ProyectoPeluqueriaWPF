using GalaSoft.MvvmLight.CommandWpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace ProyectoPeluqueria.Viewmodels
{

    class UserControlHorarioVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlHorarioRootDialogHost";

        /// <summary>
        /// Fecha que marca por defecto el calendario si no hay fecha seleccionada
        /// </summary>
        private const string FECHA_CONTROL = "01/01/0001";

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

        public ICommand ComandoBotones { get; private set; }

        /// <summary>
        /// Lista de los horarios seleccionados para habilitar o deshabilitar
        /// </summary>
        private IList _listaHorariosSeleccionados;

        public IList ListaHorariosSeleccionados
        {
            get { return _listaHorariosSeleccionados; }
            set
            {
                if (_listaHorariosSeleccionados != value)
                {
                    _listaHorariosSeleccionados = value;
                    NotifyPropertyChanged("ListaHorariosSeleccionados");
                }
            }
        }

        /// <summary>
        /// Lista de todos los horarios del establecimiento
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
        /// Horario seleccionado para habilitar o deshabilitar
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
                }
            }
        }


        /// <summary>
        /// Lista de todos los horarios deshabilitados
        /// </summary>
        private ObservableCollection<Disponibilidad> _listaHorariosNoDisponibles;
        public ObservableCollection<Disponibilidad> ListaHorariosNoDisponibles
        {
            get { return _listaHorariosNoDisponibles; }
            set
            {
                if (_listaHorariosNoDisponibles != value)
                {
                    _listaHorariosNoDisponibles = value;
                    NotifyPropertyChanged("ListaHorariosNoDisponibles");
                }
            }
        }

        /// <summary>
        /// Listado objetos seleccionados de la lista de no disponibilidad
        /// </summary>
        private ObservableCollection<Disponibilidad> _selectedItems;
        public ObservableCollection<Disponibilidad> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        /// <summary>
        /// Lista de los empleados seleccionados para habilitar o deshabilitar sus horarios
        /// </summary>
        private IList _listaEmpleadosSeleccionados;
        public IList ListaEmpleadosSeleccionados
        {
            get { return _listaEmpleadosSeleccionados; }
            set
            {
                if (_listaEmpleadosSeleccionados != value)
                {
                    _listaEmpleadosSeleccionados = value;
                    NotifyPropertyChanged("ListaEmpleadosSeleccionados");
                }
            }
        }

        /// <summary>
        /// Lista de todos los empleados del establecimiento
        /// </summary>
        private ObservableCollection<Usuario> _listaEmpleados;
        public ObservableCollection<Usuario> ListaEmpleados
        {
            get { return _listaEmpleados; }
            set
            {
                if (_listaEmpleados != value)
                {
                    _listaEmpleados = value;
                    NotifyPropertyChanged("ListaEmpleados");
                }
            }
        }

        /// <summary>
        /// Listado de citas confirmadas para comprobar si existe alguna confirmada en el horario a deshabilitar
        /// </summary>
        private ObservableCollection<Cita> _listaCitasConfirmadas;
        public ObservableCollection<Cita> ListaCitasConfirmadas
        {
            get { return _listaCitasConfirmadas; }
            set
            {
                if (_listaCitasConfirmadas != value)
                {
                    _listaCitasConfirmadas = value;
                    NotifyPropertyChanged("ListaCitasConfirmadas");
                }
            }
        }

        /// <summary>
        /// Empleado seleccionado para habilitar o deshabilitar sus horarios
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
                }
            }
        }

        /// <summary>
        /// String con los ids de los empleados seleccionados separados por coma
        /// </summary>
        private string _empleadosString;
        public string EmpleadosString
        {
            get { return _empleadosString; }
            set
            {
                if (_empleadosString != value)
                {
                    _empleadosString = value;
                    NotifyPropertyChanged("EmpleadosString");
                }
            }
        }

        /// <summary>
        /// String con los ids de los horarios seleccionados separados por coma
        /// </summary>
        private string _horariosString;
        public string HorariosString
        {
            get { return _horariosString; }
            set
            {
                if (_horariosString != value)
                {
                    _horariosString = value;
                    NotifyPropertyChanged("HorariosString");
                }
            }
        }

        /// <summary>
        /// Fecha comienzo del periodo para habilitar/deshabilitar horarios
        /// </summary>
        private DateTime _fechaComienzo;
        public DateTime FechaComienzo
        {
            get { return _fechaComienzo; }
            set
            {
                if (_fechaComienzo != value)
                {
                    _fechaComienzo = value;
                    NotifyPropertyChanged("FechaComienzo");
                }
            }
        }

        /// <summary>
        /// Fecha fin del periodo para habilitar/deshabilitar horarios
        /// </summary>
        private DateTime _fechaFin;
        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set
            {
                if (_fechaFin != value)
                {
                    _fechaFin = value;
                    NotifyPropertyChanged("FechaFin");
                }
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

        /// <summary>
        /// Obtiene string con los ids de los empleados seleccionados separados por coma a partir del listado
        /// </summary>
        /// <returns>EmpleadosString</returns>
        public string ObtieneListadoEmpleados()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ListaEmpleadosSeleccionados.Count; i++)
            {
                if (i < ListaEmpleadosSeleccionados.Count - 1)
                    sb.Append(ListaEmpleadosSeleccionados[i].ToString()).Append(",");
                else
                    sb.Append(ListaEmpleadosSeleccionados[i].ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene string con los ids de los horarios seleccionados separados por coma a partir del listado
        /// </summary>
        /// <returns>HorariosString</returns
        public string ObtieneListadoHorarios()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ListaHorariosSeleccionados.Count; i++)
            {
                if (i < ListaHorariosSeleccionados.Count - 1)
                    sb.Append(ListaHorariosSeleccionados[i].ToString()).Append(",");
                else
                    sb.Append(ListaHorariosSeleccionados[i].ToString());
            }

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene string con los ids de los objetos seleccionados del listado de no disponibilidad, separados por coma a partir del listado
        /// </summary>
        /// <returns>HorariosString</returns
        public string ObtieneListadoIdsDisponibilidad()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                if (i < SelectedItems.Count - 1)
                    sb.Append(SelectedItems[i].IdDisponibilidad).Append(",");
                else
                    sb.Append(SelectedItems[i].IdDisponibilidad);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Método CanExecute del command para deshabilitar horarios
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanModifiyDisponibilidad() => ListaEmpleadosSeleccionados.Count > 0 && ListaHorariosSeleccionados.Count > 0
            && FechaComienzo.ToShortDateString() != FECHA_CONTROL && FechaFin.ToShortDateString() != FECHA_CONTROL;

        /// <summary>
        /// Método CanExecute del command para habilitar horarios
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanHabilitarDisponibilidad() => SelectedItems.Count > 0;

        /// <summary>
        /// Método Execute del command para habilitar la disponibilidad de los horarios. Abre diálogo de confirmación y espera
        /// la respuesta de la APIRest
        /// </summary>
        public async void HabilitaDisponibilidad()
        {
            var vm = new DialogoConfirmacionVM("¿Quieres habilitar los horarios?");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                Response = ServicioApiRest.PutDelDisponibilidadIds(ObtieneListadoIdsDisponibilidad());

                if (Response.Mensaje == "Registro/s actualizado/s")
                {
                    MuestraDialogo("Disponibilidad Modificada");
                    RefrescaListas();
                }
                else
                {
                    MuestraDialogo("Ha habido un error al modificar");
                }
            }
        }

        /// <summary>
        /// Refresca todas las listas después de modificar la disponibilidad
        /// </summary>
        private void RefrescaListas()
        {
            ListaHorariosNoDisponibles = ServicioApiRest.GetHorariosDeshabilitados();
            ListaHorariosSeleccionados = new ObservableCollection<Horario>();
            ListaEmpleadosSeleccionados = new ObservableCollection<Usuario>();
            ListaHorarios = ServicioApiRest.GetHorariosLista();
            ListaEmpleados = ServicioApiRest.GetEmpleados();
            ListaCitasConfirmadas = new ObservableCollection<Cita>();
        }

        /// <summary>
        /// Método Execute del command para deshabilitar la disponibilidad de los horarios. Abre diálogo de confirmación y espera
        /// la respuesta de la APIRest
        /// </summary>
        public async void DeshabilitaDisponibilidad()
        {
            ObtieneEmpleadosYHorarios();

            var vm = new DialogoConfirmacionVM("¿Quieres deshabilitar los horarios?");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                Response = ServicioApiRest.PutAddDisponibilidad(FechaComienzo, FechaFin, EmpleadosString, HorariosString);

                if (Response.Mensaje == "Registro actualizado")
                {
                    ListaCitasConfirmadas = ServicioApiRest.GetCitasConfirmadasHorarioEmpleadoFecha(HorariosString, EmpleadosString, FechaComienzo, FechaFin);
                    if (ListaCitasConfirmadas.Count > 0)
                    {
                        MuestraDialogo("Disponibilidad Modificada. Hay una cita en esa franja horaria. Recuerde avisar al cliente");
                    }
                    else
                    {
                        MuestraDialogo("Disponibilidad Modificada");
                    }

                    RefrescaListas();
                }
                else if (Response.Mensaje == "Error registro")
                {
                    MuestraDialogo("No se puede realizar al existir previamente el registro");
                }
                else
                {
                    MuestraDialogo("Ha habido un error al modificar");
                }
            }
        }

        /// <summary>
        /// Método que abre el diálogo de selección de las fechas de selección de horarios
        /// </summary>
        public void ModificaFechasSeleccionHorarios()
        {
            DialogoFechas dialogo = new DialogoFechas();
            if (dialogo.ShowDialog() == true)
            {
                DateTime fechaComienzo = dialogo.FechasSeleccionadas[0];
                FechaComienzo = fechaComienzo;

                if (dialogo.FechasSeleccionadas[dialogo.FechasSeleccionadas.Count - 1] == null)
                {
                    FechaFin = fechaComienzo;
                }
                else
                {
                    DateTime fechaFin = dialogo.FechasSeleccionadas[dialogo.FechasSeleccionadas.Count - 1];
                    FechaFin = fechaFin;
                }
            }
        }

        /// <summary>
        /// Obtiene las dos cadenas de string necesarias
        /// </summary>
        private void ObtieneEmpleadosYHorarios()
        {
            EmpleadosString = ObtieneListadoEmpleados();
            HorariosString = ObtieneListadoHorarios();
        }


        public UserControlHorarioVM()
        {
            Response = new MensajeGeneral();
            ListaHorariosNoDisponibles = ServicioApiRest.GetHorariosDeshabilitados();
            ListaHorarios = ServicioApiRest.GetHorariosLista();
            ListaEmpleados = ServicioApiRest.GetEmpleados();
            ListaHorariosSeleccionados = new ObservableCollection<Horario>();
            ListaEmpleadosSeleccionados = new ObservableCollection<Usuario>();
            ListaCitasConfirmadas = new ObservableCollection<Cita>();

            _selectedItems = new ObservableCollection<Disponibilidad>();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
