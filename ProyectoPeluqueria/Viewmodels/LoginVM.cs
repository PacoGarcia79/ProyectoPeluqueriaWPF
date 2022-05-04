using GalaSoft.MvvmLight.CommandWpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo para autorizar a un usuario.
    /// </summary>
    class LoginVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al autorizar un usuario.
        /// </summary>
        private MensajeLogin Mensaje { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand LoginCommand { get; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    NotifyPropertyChanged("Password");
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


        public LoginVM()
        {
            Mensaje = new MensajeLogin();
            LoginCommand = new RelayCommand(OnLogin, CanLogin);
        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        private bool CanLogin() => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

        /// <summary>
        /// Método Execute de la implementación del ICommand
        /// Muestra diálogos según la respuesta de la APIRest
        /// </summary>
        private async void OnLogin()
        {
            bool result = await ValidateLogin();
            if (result)
            {
                Close?.Invoke(this, EventArgs.Empty);
                MuestraDialogo($"Bienvenido, {Mensaje.Nombre}");
            }
            else
            {
                MuestraDialogo("Introduzca los datos correctos");
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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al autorizar al usuario
        /// </summary>
        public void AutorizarUsuario()
        {
            Usuario nuevo = new Usuario(Username, Hash.ComputeSha256Hash(Password));
            Mensaje = ServicioApiRest.PostLogin(nuevo);
        }

        /// <summary>
        /// Método asíncrono que espera la respuesta de la APIRest y controla el booleano que muestra la progressbar
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateLogin()
        {
            try
            {
                IsValidating = true;

                AutorizarUsuario();

                await Task.Delay(TimeSpan.FromSeconds(2));

                return Mensaje.Nombre != "" && Mensaje.Rol == "admin";
            }
            finally
            {
                IsValidating = false;
            }
        }
    }
}
