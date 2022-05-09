using MaterialDesignThemes.Wpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using ProyectoPeluqueria.Servicios_ClasesEstaticas;

namespace ProyectoPeluqueria.Viewmodels
{
    class UserControlServiciosVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlServicioRootDialogHost";

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Instancia auxiliar para que no se pierda la referencia al servicio seleccionado al abrir el diálogo
        /// </summary>
        public Servicio ServicioSeleccionadoAuxiliar { get; set; }

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
        /// Lista de servicios del establecimiento
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
        /// Servicio seleccionado en el listbox
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

                    if (ServicioSeleccionado != null)
                    {
                        ServicioSeleccionadoAuxiliar = ServicioSeleccionado;
                    }
                }
            }
        }

        /// <summary>
        /// Foto del servicio seleccionado en Base 64
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

        public UserControlServiciosVM()
        {
            Response = new MensajeGeneral();
            ListaServicios = ServicioApiRest.GetServicios();

            ServicioSeleccionadoAuxiliar = new Servicio();
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

                ServicioSeleccionado.Foto = FotoBase64;
                ServicioSeleccionado.RutaFotoNueva = path;
            }
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al eliminar el servicio
        /// </summary>
        public void EliminarServicio()
        {
            Response = ServicioApiRest.DelServicio(ServicioSeleccionadoAuxiliar.IdServicio);
            if (Response.Mensaje == "Registro eliminado")
            {
                MuestraDialogo("Ha borrado el servicio");
            }
        }

        /// <summary>
        /// Abre el diálogo de confirmación para eliminar el servicio. Método Execute del command para eliminar el servicio
        /// </summary>
        public async void OnShowDelServicio()
        {
            var vm = new DialogoConfirmacionVM("¿Quieres eliminar el servicio?");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                EliminarServicio();
                ListaServicios = ServicioApiRest.GetServicios();
                ServicioSeleccionado = new Servicio();
            }

        }

        /// <summary>
        /// Método CanExecute del command para eliminar el servicio y para añadir el servicio al empleado
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanDeleteServiciosAddServicioEmpleado() => ServicioSeleccionado != null;

        /// <summary>
        /// Abre el diálogo para añadir el servicio al empleado. Método Execute del command para añadir el servicio al empleado
        /// </summary>
        public async void OnShowAddServicioEmpleadoForm()
        {
            var vm = new AddServicioEmpleadoVM(ServicioSeleccionadoAuxiliar.IdServicio);
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    ListaServicios = ServicioApiRest.GetServicios();
                    ServicioSeleccionadoAuxiliar = null;
                    ServicioSeleccionado = new Servicio();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }

        /// <summary>
        /// Abre el diálogo para añadir el servicio al listado. Método Execute del command para añadir el servicio
        /// </summary>
        public async void OnShowAddForm()
        {
            var vm = new AddServicioVM();
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    ListaServicios = ServicioApiRest.GetServicios();
                    ServicioSeleccionadoAuxiliar = null;
                    ServicioSeleccionado = new Servicio();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al modificar el servicio
        /// </summary>
        public void ModificarServicio()
        {
            Response = ServicioApiRest.PutServicio(ServicioSeleccionado);
        }

        /// <summary>
        /// Método CanExecute del command para modificar el servicio
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanModifyServicio() => !string.IsNullOrWhiteSpace(ServicioSeleccionadoAuxiliar.Foto) && !string.IsNullOrWhiteSpace(ServicioSeleccionadoAuxiliar.Precio.ToString())
            && !string.IsNullOrWhiteSpace(ServicioSeleccionadoAuxiliar.Nombre);

        /// <summary>
        /// Método Execute del command para modificar el servicio
        /// </summary>
        public async void OnModifyServicio()
        {
            bool result = await ValidateModify();
            if (result)
            {
                ListaServicios = ServicioApiRest.GetServicios();
                ServicioSeleccionado = new Servicio();
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
        /// Método asíncrono que controla la respuesta de la APIRest desde el método ModificarServicio
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateModify()
        {
            ModificarServicio();

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
