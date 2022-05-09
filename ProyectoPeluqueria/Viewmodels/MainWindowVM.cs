
using MaterialDesignThemes.Wpf;
using ProyectoPeluqueria.Modelos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Controls;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;
using ProyectoPeluqueria.UserControlMenu;
using GalaSoft.MvvmLight.CommandWpf;
using ProyectoPeluqueria.Viewmodels;
using ProyectoPeluqueria.DialogoPersonalizado;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace ProyectoPeluqueria
{
    /// <summary>
    /// Vista modelo de la vista principal
    /// </summary>
    class MainWindowVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "RootDialogHost";

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al hacer logout
        /// </summary>
        private MensajeLogout Mensaje { get; set; }

        /// <summary>
        /// UserControl seleccionado que se muestra en pantalla
        /// </summary>
        private UserControl _selectedUserControl;
        public UserControl SelectedUserControl
        {
            get { return _selectedUserControl; }
            set
            {
                if (_selectedUserControl != value)
                {
                    _selectedUserControl = value;
                    NotifyPropertyChanged("SelectedUserControl");
                }
            }
        }

        /// <summary>
        /// Lista de elementos del Menú Drawer
        /// </summary>
        private ObservableCollection<ElementoMenu> _listaElementosMenu;
        public ObservableCollection<ElementoMenu> ListaElementosMenu
        {
            get { return _listaElementosMenu; }
            set
            {
                if (_listaElementosMenu != value)
                {
                    _listaElementosMenu = value;
                    NotifyPropertyChanged("ListaElementosMenu");
                }
            }
        }

        /// <summary>
        /// UserControl seleccionado que se muestra en pantalla
        /// </summary>
        private ElementoMenu _elementoMenuSeleccionado;
        public ElementoMenu ElementoMenuSeleccionado
        {
            get { return _elementoMenuSeleccionado; }
            set
            {
                if (_elementoMenuSeleccionado != value)
                {
                    _elementoMenuSeleccionado = value;
                    NotifyPropertyChanged("ElementoMenuSeleccionado");
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    NotifyPropertyChanged("UserName");
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
        /// Boolean que controla el cierre del menú drawer
        /// </summary>
        private bool _cierraDrawer;
        public bool CierraDrawer
        {
            get { return _cierraDrawer; }
            set
            {
                if (_cierraDrawer != value)
                {
                    _cierraDrawer = value;
                    NotifyPropertyChanged("CierraDrawer");
                }
            }
        }

        /// <summary>
        /// Controla que el menú drawer está activo
        /// </summary>
        private bool _usuarioAutorizado;
        public bool UsuarioAutorizado
        {
            get { return _usuarioAutorizado; }
            set
            {
                if (_usuarioAutorizado != value)
                {
                    _usuarioAutorizado = value;
                    NotifyPropertyChanged("UsuarioAutorizado");
                }
            }
        }

        /// <summary>
        /// Propiedad Cliente de tipo RestClient necesaria para el Request a la APIRest
        /// </summary>
        private RestClient Client { get; set; }

        /// <summary>
        /// Propiedad Cook de tipo Cookie que se debe añadir al Request a la APIRest
        /// </summary>
        private Cookie Cook { get; set; }

        /// <summary>
        /// Obtiene la cookie necesaría para las peticiones a la APIRest
        /// </summary>
        private void GetCookie()
        {
            var request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.api);
            request.CookieContainer = new CookieContainer();

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                Cook = response.Cookies[0];
            }
        }


        public MainWindowVM()
        {
            BorraFotosDirectorioEmpleados();

            Client = new RestClient(Properties.Settings.Default.api);
            GetCookie();

            ServicioApiRest.Cook = Cook;
            ServicioApiRest.Client = Client;

            Mensaje = new MensajeLogout();

            SelectedUserControl = new UserControlInicio();

            CargaElementosMenu();

            OnLoginAsync();
        }

        /// <summary>
        /// Método que muestra el diálogo y ejecuta el logout
        /// </summary>
        public async void OnShowLogout()
        {
            var vm = new DialogoConfirmacionVM("¿Quieres cerrar la sesión?");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                Logout();
                UsuarioAutorizado = false;
                SelectedUserControl = new UserControlInicio();
            }
        }

        /// <summary>
        /// Método CanExecute de la implementación del command para login en el menu overflow
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanLoginForm() => !UsuarioAutorizado;

        /// <summary>
        /// Método Execute de la implementación del command para login en el menu overflow
        /// </summary>
        /// <returns>true/false</returns>
        public async void OnShowLoginForm()
        {
            var vm = new LoginVM();
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
             {
                 void OnClose(object _, EventArgs args)
                 {
                     UsuarioAutorizado = true;
                     vm.Close -= OnClose;
                     e.Session.Close();
                 }
                 vm.Close += OnClose;
             });
        }

        /// <summary>
        /// Método CanExecute de la implementación del command al hacer click en cada una de las opciones del menú drawer
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanUpdateUserControl() => UsuarioAutorizado;

        /// <summary>
        /// Método Execute de la implementación del command al hacer click en cada una de las opciones del menú drawer.-
        /// Modifica el UserControl seleccionado y que se mostrará en pantalla.
        /// </summary>
        /// <returns>true/false</returns>
        public void UpdateUserControl()
        {
            switch (ElementoMenuSeleccionado.Nombre.ToString())
            {
                case "Inicio":
                    SelectedUserControl = new UserControlInicio();
                    break;
                case "Productos":
                    SelectedUserControl = new UserControlProductosMain();
                    break;
                case "Agenda":
                    SelectedUserControl = new UserControlAgenda();
                    break;
                case "Citas":
                    SelectedUserControl = new UserControlCitas();
                    break;
                case "Horario":
                    SelectedUserControl = new UserControlHorario();
                    break;
                case "Empleados":
                    SelectedUserControl = new UserControlEmpleados();
                    break;
                case "Servicios":
                    SelectedUserControl = new UserControlServicios();
                    break;
            }

            CierraDrawer = false;
        }

        /// <summary>
        /// Método para cargar la ObservableCollection de elementos del menú drawer
        /// </summary>
        private void CargaElementosMenu()
        {
            ListaElementosMenu = new ObservableCollection<ElementoMenu>();

            ListaElementosMenu.Add(new ElementoMenu("Inicio", "HomeCircle", "Inicio"));
            ListaElementosMenu.Add(new ElementoMenu("Productos", "BottleTonic", "Productos"));
            ListaElementosMenu.Add(new ElementoMenu("Agenda", "Calendar", "Agenda"));
            ListaElementosMenu.Add(new ElementoMenu("Citas", "CalendarEdit", "Reserva de citas"));
            ListaElementosMenu.Add(new ElementoMenu("Horario", "StoreClock", "Horarios"));
            ListaElementosMenu.Add(new ElementoMenu("Empleados", "AccountGroup", "Empleados"));
            ListaElementosMenu.Add(new ElementoMenu("Servicios", "ContentCut", "Servicios"));
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al hacer logout
        /// </summary>
        public void Logout()
        {
            Mensaje = ServicioApiRest.PostLogout();
            if (Mensaje.Usuario == "")
            {
                MuestraDialogo("Ha salido del sistema");
            }
        }

        /// <summary>
        /// Método que carga el diálogo de login al abrir la aplicación
        /// </summary>
        public async void OnLoginAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            OnShowLoginForm();
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


        public void Salir() => Application.Current.Shutdown();

        /// <summary>
        /// Limpia directorio usado para guardar las fotos de los empleados al convertirlas desde Base64
        /// </summary>
        public void BorraFotosDirectorioEmpleados() 
        {
            string appPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            string[] paths = { appPath, "img_empleados" };
            string folderName = Path.Combine(paths);

            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            Directory.GetFiles(folderName).ToList().ForEach(File.Delete);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
