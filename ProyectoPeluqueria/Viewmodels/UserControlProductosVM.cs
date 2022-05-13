using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using ProyectoPeluqueria.Servicios_ClasesEstaticas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Productos
    /// </summary>
    class UserControlProductosVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlProductosRootDialogHost";

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Instancia auxiliar para que no se pierda la referencia al producto seleccionado al abrir el diálogo
        /// </summary>
        private Producto ProductoSeleccionadoAuxiliar { get; set; }

        /// <summary>
        /// Instancia auxiliar para que no se pierda la referencia a la cantidad seleccionada al abrir el diálogo
        /// </summary>
        private int CantidadProductoAuxiliar { get; set; }

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

        /// <summary>
        /// Nombre del grupo de productos usado para obtener el listado de productos
        /// </summary>
        private string _tipoProductoGrupo;
        public string TipoProductoGrupo
        {
            get { return _tipoProductoGrupo; }
            set
            {
                if (_tipoProductoGrupo != value)
                {
                    _tipoProductoGrupo = value;
                    NotifyPropertyChanged("TipoProductoGrupo");
                }
            }
        }

        /// <summary>
        /// Lista de todos los productos de un grupo determinado
        /// </summary>
        private ObservableCollection<Producto> _listaProductos;
        public ObservableCollection<Producto> ListaProductos
        {
            get { return _listaProductos; }
            set
            {
                if (_listaProductos != value)
                {
                    _listaProductos = value;
                    NotifyPropertyChanged("ListaProductos");
                }
            }
        }

        /// <summary>
        /// Producto seleccionado del listado de productos
        /// </summary>
        private Producto _productoSeleccionado;
        public Producto ProductoSeleccionado
        {
            get { return _productoSeleccionado; }
            set
            {
                if (_productoSeleccionado != value)
                {
                    _productoSeleccionado = value;
                    NotifyPropertyChanged("ProductoSeleccionado");

                    if (ProductoSeleccionado != null)
                    {
                        ProductoSeleccionadoAuxiliar = ProductoSeleccionado;
                    }
                }
            }
        }

        /// <summary>
        /// Foto del empleado seleccionado en Base 64
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

        public UserControlProductosVM()
        {
            TipoProductoGrupo = NavegacionTipoProducto.TipoProducto;
            Response = new MensajeGeneral();
            ListaProductos = ServicioApiRest.GetProductosGrupo(TipoProductoGrupo);

            ProductoSeleccionadoAuxiliar = new Producto();
        }

        /// <summary>
        /// Método Execute de la implementación del Command
        /// Abre el diálogo para seleccionar la foto, y una vez seleccionada la convierte en Base64      
        /// </summary>
        public void OnOpenDialog()
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            bool? result = open.ShowDialog();

            if (result == true)
            {
                string path = open.FileName;

                FotoBase64 = ImgUtils.imgToBase64(path);

                ProductoSeleccionado.Foto = FotoBase64;
                ProductoSeleccionado.RutaFotoNueva = path;
            }
        }


        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al eliminar el producto
        /// </summary>
        public void EliminarProducto()
        {
            var response = ServicioApiRest.DelProducto(ProductoSeleccionadoAuxiliar.IdProducto);
            if (response != null)
            {
                Response = response;

                if (Response.Mensaje == "Registro eliminado")
                {
                    MuestraDialogo("Ha borrado el producto");
                }
            }                
            else
                Response = new MensajeGeneral("Error de acceso a la base de datos");
            
        }


        /// <summary>
        /// Método CanExecute del command para añadir el producto a la cita
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanDeleteProductos() => ProductoSeleccionado != null;

        /// <summary>
        /// Abre el diálogo de confirmación para eliminar el producto
        /// </summary>
        public async void OnShowDelProducto()
        {
            var vm = new DialogoConfirmacionVM("¿Quieres eliminar el producto?");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                EliminarProducto();
                ListaProductos = ServicioApiRest.GetProductosGrupo(TipoProductoGrupo);
                ProductoSeleccionado = new Producto();
            }
        }

        /// <summary>
        /// Abre el diálogo para añadir el producto al listado
        /// </summary>
        public async void OnShowAddForm()
        {
            int valorId = ServicioApiRest.GetIdGrupo(TipoProductoGrupo);
            var vm = new AddProductoVM(valorId);
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    ListaProductos = ServicioApiRest.GetProductosGrupo(TipoProductoGrupo);
                    ProductoSeleccionadoAuxiliar = null;
                    ProductoSeleccionado = new Producto();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }

        /// <summary>
        /// Método CanExecute del command para añadir el producto a la cita
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAddProductoCita() => ProductoSeleccionadoAuxiliar.Cantidad != 0;
        

        /// <summary>
        /// Abre el diálogo para añadir el producto a la cita. Método Execute del command para añadir el producto a la cita
        /// </summary>
        public async void OnShowAddProductoCitaForm()
        {
            CantidadProductoAuxiliar = ProductoSeleccionado.Cantidad;
            var vm = new AddProductoCitaVM(ProductoSeleccionadoAuxiliar.IdProducto, CantidadProductoAuxiliar);
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    ListaProductos = ServicioApiRest.GetProductosGrupo(TipoProductoGrupo);
                    ProductoSeleccionadoAuxiliar = null;
                    ProductoSeleccionado = new Producto();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }

        /// <summary>
        /// Método CanExecute del command para modificar un producto
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanModifyProductos() => ProductoSeleccionado != null && !string.IsNullOrWhiteSpace(ProductoSeleccionado.Foto) && !string.IsNullOrWhiteSpace(ProductoSeleccionado.Precio.ToString())
            && !string.IsNullOrWhiteSpace(ProductoSeleccionado.Nombre) && !string.IsNullOrWhiteSpace(ProductoSeleccionado.Descripcion) && ProductoSeleccionado.Precio > 0;

        /// <summary>
        /// Método Execute del command para modificar un producto
        /// </summary>
        public async void OnModifyProductos()
        {
            bool result = await ValidateModify();
            if (result)
            {
                ListaProductos = ServicioApiRest.GetProductosGrupo(TipoProductoGrupo);
                ProductoSeleccionado = new Producto();
            }

            MuestraDialogo($"{Response.Mensaje}");
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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al modificar el producto
        /// </summary>
        public void ModificarProducto()
        {
            var response = ServicioApiRest.PutProducto(ProductoSeleccionado);
            if (response != null)
                Response = response;
            else
                Response = new MensajeGeneral("Error de acceso a la base de datos");
        }

        /// <summary>
        /// Método asíncrono que controla la respuesta de la APIRest desde el método ModificarProducto
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateModify()
        {
            ModificarProducto();

            await Task.Delay(TimeSpan.FromSeconds(1));

            return Response.Mensaje == "Registro actualizado";

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
