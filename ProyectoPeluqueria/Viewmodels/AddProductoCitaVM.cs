
using GalaSoft.MvvmLight.CommandWpf;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del diálogo para añadir un producto a una cita.
    /// </summary>
    class AddProductoCitaVM : INotifyPropertyChanged
    {
        public event EventHandler Close;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest al añadir el producto a la cita
        /// </summary>
        private MensajeGeneral Response { get; set; }
        
        /// <summary>
        /// Id del producto seleccionado que se quiere añadir
        /// </summary>
        private int IdProductoSeleccionado { get; set; }

        /// <summary>
        /// Cantidad del producto seleccionado que se quiere añadir
        /// </summary>
        private int CantidadProductoSeleccionado { get; set; }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddProductoCitaCommand { get; }

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
        /// Lista de Citas que muestra el combobox para añadir los productos
        /// </summary>
        private ObservableCollection<Cita> _listaCitas;
        public ObservableCollection<Cita> ListaCitas
        {
            get { return _listaCitas; }
            set
            {
                if (_listaCitas != value)
                {
                    _listaCitas = value;
                    NotifyPropertyChanged("ListaCitas");
                }
            }
        }
        
        /// <summary>
        /// Cita seleccionada para añadir el producto
        /// </summary>
        private Cita _citaSeleccionada;
        public Cita CitaSeleccionada
        {
            get { return _citaSeleccionada; }
            set
            {
                if (_citaSeleccionada != value)
                {
                    _citaSeleccionada = value;
                    NotifyPropertyChanged("CitaSeleccionada");
                }
            }
        }

        
        public AddProductoCitaVM(int idProductoSeleccionado, int cantidad)
        {
            Response = new MensajeGeneral();
            IdProductoSeleccionado = idProductoSeleccionado;
            CantidadProductoSeleccionado = cantidad;

            ListaCitas = ServicioApiRest.GetCitas(DateTime.Now, DateTime.Now.AddDays(15),0);

            AddProductoCitaCommand = new RelayCommand(OnAdd, CanAdd);
        }

        /// <summary>
        /// Método CanExecute de la implementación del ICommand
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanAdd() => CitaSeleccionada != null;


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
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al añadir el producto a la cita
        /// </summary>
        public void AddCitaProducto()
        {
            var response = ServicioApiRest.PostProductoCita(CitaSeleccionada.IdCita, IdProductoSeleccionado, CantidadProductoSeleccionado);
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

                AddCitaProducto();

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
