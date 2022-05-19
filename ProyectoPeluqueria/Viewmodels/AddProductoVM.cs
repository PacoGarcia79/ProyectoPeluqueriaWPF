using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProyectoPeluqueria.Servicios_ClasesEstaticas;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo para añadir un producto.
    /// </summary>
    class AddProductoVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir un empleado
        /// </summary>
        private MensajeGeneral Response { get; set; }

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

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddProductoCommand { get; }
        public ICommand OpenDialogCommand { get; }

        /// <summary>
        /// Producto nuevo a añadir
        /// </summary>
        private Producto _productoNuevo;
        public Producto ProductoNuevo
        {
            get { return _productoNuevo; }
            set
            {
                if (_productoNuevo != value)
                {
                    _productoNuevo = value;
                    NotifyPropertyChanged("ProductoNuevo");
                }
            }
        }

        /// <summary>
        /// Id del grupo de productos que se settea al producto a añadir
        /// </summary>
        private int _valorId;
        public int ValorId
        {
            get { return _valorId; }
            set
            {
                if (_valorId != value)
                {
                    _valorId = value;
                    NotifyPropertyChanged("ValorId");
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
        /// Foto del producto en Base 64
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

        public AddProductoVM(int valorId)
        {
            ValorId = valorId;
            ProductoNuevo = new Producto();
            Response = new MensajeGeneral();
            AddProductoCommand = new RelayCommand(OnAdd, CanAdd);
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

                ProductoNuevo.Foto = FotoBase64;
                RutaFotoNueva = path;
            }

        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => !string.IsNullOrWhiteSpace(ProductoNuevo.Foto) && !string.IsNullOrWhiteSpace(ProductoNuevo.Precio.ToString())
            && !string.IsNullOrWhiteSpace(ProductoNuevo.Nombre) && !string.IsNullOrWhiteSpace(ProductoNuevo.Descripcion);

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
                MuestraDialogo("Producto añadido");
            }
            else
            {
                if (Response.Mensaje == "Debes identificarte")
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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir el producto
        /// </summary>
        public void AddProducto()
        {
            ProductoNuevo.IdProductoGrupo = ValorId;

            var response = ServicioApiRest.PostProducto(ProductoNuevo);
            if (response != null)
                Response = response;
            else
            {
                Response = new MensajeGeneral("Error de acceso a la base de datos");
                MuestraDialogo(Response.Mensaje);
            }
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

                AddProducto();

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
