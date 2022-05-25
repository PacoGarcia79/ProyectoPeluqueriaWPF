using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using ProyectoPeluqueria.Servicios_ClasesEstaticas;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Empleados
    /// </summary>
    class UserControlEmpleadosVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlEmpleadoRootDialogHost";

        private static Random random = new Random();

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

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
        /// Instancia auxiliar para que no se pierda la referencia al empleado seleccionado al abrir el diálogo
        /// </summary>
        private Usuario EmpleadoSeleccionadoAuxiliar { get; set; }

        /// <summary>
        /// Lista de empleados que se muestra en la ventana
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
        /// Empleado seleccionado para dar de baja o modificar
        /// </summary>
        private Usuario _empleadoSeleccionado;
        public Usuario EmpleadoSeleccionado
        {
            get { return _empleadoSeleccionado; }
            set
            {
                if (_empleadoSeleccionado != value)
                {
                    _empleadoSeleccionado = value;
                    NotifyPropertyChanged("EmpleadoSeleccionado");

                    if (EmpleadoSeleccionado != null)
                    {
                        EmpleadoSeleccionadoAuxiliar = EmpleadoSeleccionado;
                    }
                }
            }
        }

        public UserControlEmpleadosVM()
        {
            Response = new MensajeGeneral();
            CargaEmpleados();

            EmpleadoSeleccionadoAuxiliar = new Usuario();
        }

        /// <summary>
        /// Carga el listado de empleados y muestra mensaje si es null o no contiene elementos
        /// </summary>
        public async void CargaEmpleados()
        {
            ListaEmpleados = ServicioApiRest.GetEmpleados();

            await Task.Delay(TimeSpan.FromSeconds(1));

            if (ListaEmpleados == null)
            {
                MuestraDialogo("Error al obtener los datos");
            }
            else if (ListaEmpleados.Count == 0)
            {
                MuestraDialogo("No se han obtenido empleados");
            }

        }

        /// <summary>
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

                EmpleadoSeleccionado.Foto = FotoBase64;
                EmpleadoSeleccionado.RutaFotoNueva = path;
            }
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al dar de baja al empleado
        /// </summary>
        public void BajaEmpleado()
        {
            EmpleadoSeleccionadoAuxiliar.Fecha_Baja = DateTime.Now;

            var response = ServicioApiRest.PutEmpleadoFechaBaja(EmpleadoSeleccionadoAuxiliar);
            if (response != null)
            {
                Response = response;

                if (Response.Mensaje == "Registro actualizado")
                {
                    MuestraDialogo("Ha dado de baja al empleado");
                }
                else if (Response.Mensaje == "Debes identificarte")
                {
                    Properties.Settings.Default.autorizado = false;
                    MuestraDialogo("Debe volver a iniciar sesión");
                }
            }
            else
            {
                Response = new MensajeGeneral("Error de acceso a la base de datos");
                MuestraDialogo(Response.Mensaje);
            }
        }

        /// <summary>
        /// Método CanExecute del command para dar de baja al empleado
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanBajaEmpleado() => EmpleadoSeleccionado != null;

        /// <summary>
        /// Abre diálogo de confirmación para dar de baja al empleado
        /// </summary>
        public async void OnShowBajaEmpleado()
        {
            var vm = new DialogoConfirmacionVM("¿Quieres dar de baja al empleado? Por favor, compruebe antes si tiene citas confirmadas.");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                BajaEmpleado();
                if (Response.Mensaje == "Registro actualizado")
                {
                    CargaEmpleados();
                    EmpleadoSeleccionado = new Usuario();
                }                    
            }
        }

        /// <summary>
        /// Abre el diálogo para añadir el empleado. 
        /// </summary>
        public async void OnShowAddForm()
        {
            var vm = new AddEmpleadoVM();
            await MDIXDialogHost.Show(vm, DialogIdentifier, (object sender, DialogOpenedEventArgs e) =>
            {
                void OnClose(object _, EventArgs args)
                {
                    CargaEmpleados();
                    vm.Close -= OnClose;
                    e.Session.Close();
                }
                vm.Close += OnClose;
            });
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al modificar los datos del empleado, incluyendo la contraseña
        /// </summary>
        public void ModificarEmpleadoContraseña()
        {
            EmpleadoSeleccionado.Password = Hash.ComputeSha256Hash(EmpleadoSeleccionado.Password);
            Response = ServicioApiRest.PutEmpleadoPassw(EmpleadoSeleccionado);
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al modificar los datos del empleado, sin incluir la contraseña
        /// </summary>
        public void ModificarEmpleado()
        {
            Response = ServicioApiRest.PutEmpleado(EmpleadoSeleccionado);
        }

        /// <summary>
        /// Método CanExecute del command para modificar los datos del empleado
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanModifyEmpleado()
        {
            if (EmpleadoSeleccionadoAuxiliar.ModificaContraseña)
            {
                return !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Username) && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Password)
                && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Nombre) && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Apellidos)
                && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Foto) && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.ConfirmaContraseña);
            }
            else
            {
                return !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Username) && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Nombre)
                    && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Apellidos) && !string.IsNullOrWhiteSpace(EmpleadoSeleccionadoAuxiliar.Foto);
            }
        }

        /// <summary>
        /// Método Execute del command para modificar los datos del empleado
        /// </summary>
        /// <returns>true/false</returns>
        public async void OnModifyEmpleado()
        {
            if (EmpleadoSeleccionado.Password != EmpleadoSeleccionado.ConfirmaContraseña && EmpleadoSeleccionadoAuxiliar.ModificaContraseña)
            {
                MuestraDialogo("Las contraseñas no coinciden");
            }
            else
            {
                bool result = await ValidateModify();
                if (result)
                {
                    CargaEmpleados();
                    EmpleadoSeleccionado = new Usuario();
                    FotoBase64 = "";
                }

                MuestraDialogo($"{Response.Mensaje}");
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
        /// Método asíncrono que controla la respuesta de la APIRest desde los métodos ModificarEmpleado y ModificarEmpleadoContraseña
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ValidateModify()
        {
            if (EmpleadoSeleccionado.ModificaContraseña)
            {
                ModificarEmpleadoContraseña();
            }
            else
            {
                ModificarEmpleado();
            }

            await Task.Delay(TimeSpan.FromSeconds(1));

            if (Response != null)
            {
                if (Response.Mensaje == "Registro actualizado")
                    return true;
                else
                {
                    Properties.Settings.Default.autorizado = false;
                    return false;
                }
            }
            else
            {
                Response = new MensajeGeneral("Error al acceder a a la base de datos");
                return false;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

