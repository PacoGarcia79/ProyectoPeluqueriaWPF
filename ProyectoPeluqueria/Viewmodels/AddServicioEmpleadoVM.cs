using GalaSoft.MvvmLight.CommandWpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo para añadir un servicio a un empleado.
    /// </summary>
    class AddServicioEmpleadoVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir el servicio a un empleado
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Id del servicio seleccionado que se quiere añadir
        /// </summary>
        private int IdServicioSeleccionado { get; set; }


        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddServicioEmpleadoCommand { get; }

        /// <summary>
        /// Boolean que gestiona la muestra de la progressbar
        /// </summary>
        private bool _isValidating;
        public bool IsValidating
        {
            get { return _isValidating; }
            set
            {
                if (_isValidating != value)
                {
                    _isValidating = value;
                    NotifyPropertyChanged("IsValidating");
                }
            }
        }

        /// <summary>
        /// Lista de empleados que muestra el combobox para añadir los servicios
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
        /// Empleado seleccionado
        /// </summary>
        private Usuario _usuarioSeleccionado;
        public Usuario UsuarioSeleccionado
        {
            get { return _usuarioSeleccionado; }
            set
            {
                if (_usuarioSeleccionado != value)
                {
                    _usuarioSeleccionado = value;
                    NotifyPropertyChanged("UsuarioSeleccionado");
                }
            }
        }


        public AddServicioEmpleadoVM(int idServicioSeleccionado)
        {
            Response = new MensajeGeneral();
            this.IdServicioSeleccionado = idServicioSeleccionado;

            ListaEmpleados = ServicioApiRest.GetEmpleadosFaltaServicios(IdServicioSeleccionado);

            AddServicioEmpleadoCommand = new RelayCommand(OnAdd, CanAdd);
        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => UsuarioSeleccionado != null;

        /// <summary>
        /// Método Execute de la implementación del ICommand
        /// Muestra diálogos según la respuesta de la APIRest
        /// </summary>
        private async void OnAdd()
        {
            bool result = await ValidateAdd();
            if (result)
            {
                Close?.Invoke(this, EventArgs.Empty);
                MuestraDialogo("Servicio añadido");
            }
            else
            {
                MuestraDialogo("No se ha podido añadir");
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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir el servicio al empleado
        /// </summary>
        public void AddServicioEmpleado()
        {
            Response = ServicioApiRest.PutEmpleadoServicio(UsuarioSeleccionado.IdUsuario, IdServicioSeleccionado);
        }

        /// <summary>
        /// Método asíncrono que espera la respuesta de la APIRest y controla el booleano que muestra la progressbar
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateAdd()
        {
            try
            {
                IsValidating = true;

                AddServicioEmpleado();

                await Task.Delay(TimeSpan.FromSeconds(2));

                return Response.Mensaje == "Registro actualizado";
            }
            finally
            {
                IsValidating = false;
            }
        }
    }
}
