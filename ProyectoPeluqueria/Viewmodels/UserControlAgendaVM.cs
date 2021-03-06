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
using MDIXDialogHost = MaterialDesignThemes.Wpf.DialogHost;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Agenda
    /// </summary>
    class UserControlAgendaVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Gestiona mensaje de respuesta de la petición al APIRest
        /// </summary>
        private MensajeGeneral Response { get; set; }

        /// <summary>
        /// Identificador del DialogHost
        /// </summary>
        private const string DialogIdentifier = "UserControlAgendaRootDialogHost";

        /// <summary>
        /// Lista de Citas confirmadas que se muestra en la ventana
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
        /// Listado objetos seleccionados de la lista de citas
        /// </summary>
        private ObservableCollection<Cita> _selectedItems;
        public ObservableCollection<Cita> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    NotifyPropertyChanged("Selected");
                }
            }
        }

        /// <summary>
        /// Fecha de comienzo del periodo de muestra de citas
        /// </summary>
        private DateTime _fechaComienzo;
        public DateTime FechaComienzo
        {
            get { return _fechaComienzo; }
            set
            {
                if (_fechaComienzo != value)
                {
                    _fechaComienzo = value;
                    NotifyPropertyChanged("FechaComienzo");
                }
            }
        }

        /// <summary>
        /// Fecha de fin del periodo de muestra de citas
        /// </summary>
        private DateTime _fechaFin;
        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set
            {
                if (_fechaFin != value)
                {
                    _fechaFin = value;
                    NotifyPropertyChanged("FechaFin");
                }
            }
        }

        /// <summary>
        /// Obtiene string con los ids de los objetos seleccionados del listado de citas, separados por coma a partir del listado
        /// </summary>
        /// <returns>HorariosString</returns
        public string ObtieneListadoIdsCitas()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                if (i < SelectedItems.Count - 1)
                    sb.Append(SelectedItems[i].IdCita).Append(",");
                else
                    sb.Append(SelectedItems[i].IdCita);
            }

            return sb.ToString();
        }


        public UserControlAgendaVM()
        {
            Response = new MensajeGeneral();
            FechaComienzo = DateTime.Now;
            FechaFin = DateTime.Now.AddDays(60);
            CargaCitas();

            _selectedItems = new ObservableCollection<Cita>();
        }

        /// <summary>
        /// Carga las citas confirmadas y muestra mensaje si no hay ningúna confirmada en ese rango de fechas
        /// </summary>
        public async void CargaCitas()
        {
            ListaCitas = ServicioApiRest.GetCitas(FechaComienzo, FechaFin, 0);

            await Task.Delay(TimeSpan.FromSeconds(1));

            if (ListaCitas == null)
            {
                MuestraDialogo("Error al obtener los datos");
            }
            else if (ListaCitas.Count == 0)
            {
                MuestraDialogo("No hay citas reservadas en este rango de fechas");
            }
            
        }

        /// <summary>
        /// Método que abre el diálogo de selección de las fechas para las que se mostrarán las citas
        /// Si solo se selecciona una cita, la fecha de comienzo será igual a la fecha de fin
        /// </summary>
        public void ModificaFechasMuestraCitas()
        {
            DialogoFechas dialogo = new DialogoFechas();
            if (dialogo.ShowDialog() == true)
            {
                DateTime fechaComienzo = dialogo.FechasSeleccionadas[0];
                FechaComienzo = fechaComienzo;

                if (dialogo.FechasSeleccionadas[dialogo.FechasSeleccionadas.Count - 1] == null)
                {
                    FechaFin = fechaComienzo;
                }
                else
                {
                    DateTime fechaFin = dialogo.FechasSeleccionadas[dialogo.FechasSeleccionadas.Count - 1];
                    FechaFin = fechaFin;
                }

                CargaCitas();
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
        /// Método Execute del command que abre el diálogo para cancelar la cita
        /// </summary>
        public async void OnCancelCita()
        {
            var vm = new DialogoConfirmacionVM("¿Quieres cancelar las citas?");
            object dialogResult = await MDIXDialogHost.Show(vm, DialogIdentifier);
            if (dialogResult is bool boolResult && boolResult)
            {
                BajaCita();
                if (Response.Mensaje == "Registro/s actualizado/s")
                {
                    CargaCitas();
                }                    
            }
        }

        /// <summary>
        /// Método que recibe la respuesta de la APIRest a través del ServicioAPIRest al dar de baja la cita
        /// </summary>
        public void BajaCita()
        {
            var response = ServicioApiRest.PutCitaCancelada(ObtieneListadoIdsCitas());
            if (response != null)
            {
                Response = response;

                if (Response.Mensaje == "Registro/s actualizado/s")
                {
                    MuestraDialogo("Ha cancelado las citas");
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
        /// Método CanExecute del command que abre el diálogo para cancelar la cita
        /// </summary>
        /// <returns>true/false</returns>
        public bool CanCancelCita() => SelectedItems.Count > 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
