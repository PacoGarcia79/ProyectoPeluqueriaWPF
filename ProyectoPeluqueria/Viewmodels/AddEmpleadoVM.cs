using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using ProyectoPeluqueria.Servicios_ClasesEstaticas;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo para añadir un empleado.
    /// </summary>
    class AddEmpleadoVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        private static Random random = new Random();

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir un empleado
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir horarios al empleado
        /// </summary>
        private MensajeGeneral ResponseAux { get; set; }

        /// <summary>
        /// Foto del empleado en Base 64
        /// </summary>
        private string _fotoBase64;
        public string FotoBase64
        {
            get { return _fotoBase64; }
            set
            {
                if (_fotoBase64 != value)
                {
                    _fotoBase64 = value;
                    NotifyPropertyChanged("FotoBase64");
                }
            }
        }

        /// <summary>
        /// Ruta de la foto a añadir
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

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddEmpleadoCommand { get; }

        public ICommand OpenDialogCommand { get; }

        /// <summary>
        /// Empleado a añadir
        /// </summary>
        private Usuario _usuarioNuevo;
        public Usuario UsuarioNuevo
        {
            get { return _usuarioNuevo; }
            set
            {
                if (_usuarioNuevo != value)
                {
                    _usuarioNuevo = value;
                    NotifyPropertyChanged("UsuarioNuevo");
                }
            }
        }

        /// <summary>
        /// Fecha usada para el alta del empleado
        /// </summary>
        private DateTime _fechaActual;
        public DateTime FechaActual
        {
            get { return _fechaActual; }
            set
            {
                if (_fechaActual != value)
                {
                    _fechaActual = value;
                    NotifyPropertyChanged("FechaActual");
                }
            }
        }

        /// <summary>
        /// String confirmación del password
        /// </summary>
        private string _confirmacionPassword;
        public string ConfirmacionPassword
        {
            get { return _confirmacionPassword; }
            set
            {
                if (_confirmacionPassword != value)
                {
                    _confirmacionPassword = value;
                    NotifyPropertyChanged("ConfirmacionPassword");
                }
            }
        }

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


        public AddEmpleadoVM()
        {
            UsuarioNuevo = new Usuario();
            Response = new MensajeGeneral();
            ResponseAux = new MensajeGeneral();
            FechaActual = DateTime.Now;
            AddEmpleadoCommand = new RelayCommand(OnAdd, CanAdd);
            OpenDialogCommand = new RelayCommand(OnOpenDialog);
        }


        /// <summary>
        /// Método Execute de la implementación del ICommand OpenDialogCommand
        /// Abre el diálogo para seleccionar la foto, y una vez seleccionada la convierte en Base64
        /// </summary>
        private void OnOpenDialog()
        {

            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                string path = open.FileName;

                FotoBase64 = ImgUtils.imgToBase64(path);

                UsuarioNuevo.Foto = FotoBase64;
                RutaFotoNueva = path;
            }

        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => !string.IsNullOrWhiteSpace(UsuarioNuevo.Nombre) && !string.IsNullOrWhiteSpace(UsuarioNuevo.Foto)
            && !string.IsNullOrWhiteSpace(UsuarioNuevo.Apellidos) && !string.IsNullOrWhiteSpace(UsuarioNuevo.Username)
            && !string.IsNullOrWhiteSpace(UsuarioNuevo.Password) && !string.IsNullOrWhiteSpace(UsuarioNuevo.Apellidos)
            && !string.IsNullOrWhiteSpace(UsuarioNuevo.Email) && !string.IsNullOrWhiteSpace(UsuarioNuevo.Telefono) && !string.IsNullOrWhiteSpace(RutaFotoNueva)
            && !string.IsNullOrWhiteSpace(ConfirmacionPassword) && ConfirmacionPassword == UsuarioNuevo.Password;

        /// <summary>
        /// Método Execute de la implementación del ICommand
        /// Muestra diálogos según la respuesta de la APIRest
        /// </summary>
        private async void OnAdd()
        {
            bool result = await ValidateAdd();
            if (result)
            {
                FechaActual = DateTime.Now;
                MuestraDialogo("Empleado añadido");
                Close?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                if (Response.Mensaje == "Ya existe el usuario")
                {
                    MuestraDialogo(Response.Mensaje);
                }
                else if(Response.Mensaje == "Debes identificarte")
                {
                    Properties.Settings.Default.autorizado = false;
                    MuestraDialogo("Ha habido un problema y debes iniciar sesión");
                }
                else
                {
                    MuestraDialogo("No se ha podido añadir");
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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir el empleado
        /// </summary>
        public void AddEmpleado()
        {
            UsuarioNuevo.Password = Hash.ComputeSha256Hash(UsuarioNuevo.Password);
            UsuarioNuevo.Fecha_Alta = FechaActual;

            var response = ServicioApiRest.PostUsuario(UsuarioNuevo, FotoBase64);
            if (response != null)
                Response = response;
            else
            {
                Response = new MensajeGeneral("Error de acceso a la base de datos");
                MuestraDialogo(Response.Mensaje);
            }
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir los horarios al empleado
        /// </summary>
        public void AddHorariosEmpleado()
        {
            ResponseAux = ServicioApiRest.PutEmpleadoHorarios();
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

                AddEmpleado();

                if(Response.Mensaje == "Registro insertado")
                {
                    AddHorariosEmpleado();
                }

                await Task.Delay(TimeSpan.FromSeconds(2));

                return Response.Mensaje == "Registro insertado" && ResponseAux.Mensaje == "Registro actualizado";
            }
            catch (NullReferenceException)
            {
                MuestraDialogo("No se puede acceder a la base de datos");
                return false;
            }
            finally
            {
                IsValidating = false;
            }
        }
    }
}


