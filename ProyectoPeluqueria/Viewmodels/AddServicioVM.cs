using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using ProyectoPeluqueria.Servicios_ClasesEstaticas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Input;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo para añadir un servicio.
    /// </summary>
    class AddServicioVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir el servicio
        /// </summary>
        private MensajeGeneral Response { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Nombre de la foto
        /// </summary>
        private string _nombreFoto;
        public string NombreFoto
        {
            get { return _nombreFoto; }
            set
            {
                if (_nombreFoto != value)
                {
                    _nombreFoto = value;
                    NotifyPropertyChanged("NombreFoto");
                }
            }
        }

        public ICommand AddServicioCommand { get; }
        public ICommand OpenDialogCommand { get; }

        /// <summary>
        /// Servicio a añadir
        /// </summary>
        private Servicio _servicioNuevo;
        public Servicio ServicioNuevo
        {
            get { return _servicioNuevo; }
            set
            {
                if (_servicioNuevo != value)
                {
                    _servicioNuevo = value;
                    NotifyPropertyChanged("ServicioNuevo");
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

        /// <summary>
        /// Foto del servicio en Base 64
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


        public AddServicioVM()
        {
            ServicioNuevo = new Servicio();
            Response = new MensajeGeneral();
            AddServicioCommand = new RelayCommand(OnAdd, CanAdd);
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

                ServicioNuevo.Foto = FotoBase64;
                RutaFotoNueva = path;
            }
        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => !string.IsNullOrWhiteSpace(RutaFotoNueva) && !string.IsNullOrWhiteSpace(ServicioNuevo.Precio.ToString())
            && !string.IsNullOrWhiteSpace(ServicioNuevo.Nombre);

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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir el servicio 
        /// </summary>
        public void AddServicio()
        {
            var response = ServicioApiRest.PostServicio(ServicioNuevo);
            if (response != null)
                Response = response;
            else
                Response = new MensajeGeneral("Error de acceso a la base de datos");
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

                AddServicio();

                await Task.Delay(TimeSpan.FromSeconds(2));

                return Response.Mensaje == "Registro insertado";
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
