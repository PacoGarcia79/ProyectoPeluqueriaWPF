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
    /// Vista modelo del diálogo para añadir un grupo de productos.
    /// </summary>
    class AddProductoGrupoVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir un grupo de productos
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

        public ICommand OpenDialogCommand { get; }
        public ICommand AddGrupoCommand { get; }

        /// <summary>
        /// Grupo de productos a añadir
        /// </summary>
        private ProductoGrupo _grupoNuevo;
        public ProductoGrupo GrupoNuevo
        {
            get { return _grupoNuevo; }
            set
            {
                if (_grupoNuevo != value)
                {
                    _grupoNuevo = value;
                    NotifyPropertyChanged("GrupoNuevo");
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

        /// <summary>
        /// Foto del empleado seleccionado. En Base 64
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


        public AddProductoGrupoVM()
        {
            GrupoNuevo = new ProductoGrupo();
            Response = new MensajeGeneral();
            AddGrupoCommand = new RelayCommand(OnAdd, CanAdd);
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

                RutaFotoNueva = path;
            }
        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => !string.IsNullOrWhiteSpace(GrupoNuevo.NombreGrupo)
            && !string.IsNullOrWhiteSpace(GrupoNuevo.Foto);

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
                MuestraDialogo("Grupo añadido");
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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir el grupo de productos
        /// </summary>
        public void AddGrupo()
        {
            Response = ServicioApiRest.PostProductoGrupo(GrupoNuevo, FotoBase64);
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

                AddGrupo();

                await Task.Delay(TimeSpan.FromSeconds(2));

                return Response.Mensaje == "Registro insertado";
            }
            finally
            {
                IsValidating = false;
            }
        }
    }
}
