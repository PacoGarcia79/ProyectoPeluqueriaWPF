using MaterialDesignThemes.Wpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using ProyectoPeluqueria.UserControlMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Productos Grupos
    /// </summary>
    class UserControlProductosGruposVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlProductosGrupoRootDialogHost";

        /// <summary>
        /// Instancia auxiliar para que no se pierda la referencia l grupo de productos seleccionado al abrir el diálogo
        /// </summary>
        ProductoGrupo ProductoGrupoSeleccionadoAuxiliar { get; set; }

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Listado de todos los grupos de productos
        /// </summary>
        private ObservableCollection<ProductoGrupo> _listaProductosGrupos;
        public ObservableCollection<ProductoGrupo> ListaProductosGrupos
        {
            get { return _listaProductosGrupos; }
            set
            {
                if (_listaProductosGrupos != value)
                {
                    _listaProductosGrupos = value;
                    NotifyPropertyChanged("ListaProductosGrupos");
                }
            }
        }

        /// <summary>
        /// Grupo de productos seleccionado en el listbox
        /// </summary>
        private ProductoGrupo _productoGrupoSeleccionado;
        public ProductoGrupo ProductoGrupoSeleccionado
        {
            get { return _productoGrupoSeleccionado; }
            set
            {
                if (_productoGrupoSeleccionado != value)
                {
                    _productoGrupoSeleccionado = value;
                    NotifyPropertyChanged("ProductoGrupoSeleccionado");
                }
            }
        }

        /// <summary>
        /// Abre el diálogo para añadir el grupo de productos. 
        /// </summary>
        public async void OnShowAddForm()
        {
            var vm = new AddProductoGrupoVM();
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    CargaProductos();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }

        /// <summary>
        /// Abre el diálogo para modificar el grupo de productos. 
        /// </summary>
        public async void OnShowModifyForm()
        {
            ProductoGrupoSeleccionadoAuxiliar = ProductoGrupoSeleccionado;
            var vm = new ModifyProductoGrupoVM(ProductoGrupoSeleccionadoAuxiliar);
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    CargaProductos();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }


        public UserControlProductosGruposVM()
        {
            ProductoGrupoSeleccionado = new ProductoGrupo();
            Response = new MensajeGeneral();
            CargaProductos();

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
        /// Carga el listado de grupos de productos y muestra mensaje si es null o no contiene elementos
        /// </summary>
        private void CargaProductos()
        {
            ListaProductosGrupos = ServicioApiRest.GetGrupos();

            if (ListaProductosGrupos == null)
            {
                MuestraDialogo("Error al obtener los datos");
            }
            else if (ListaProductosGrupos.Count == 0)
            {
                MuestraDialogo("No se han obtenido grupos de productos");
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
