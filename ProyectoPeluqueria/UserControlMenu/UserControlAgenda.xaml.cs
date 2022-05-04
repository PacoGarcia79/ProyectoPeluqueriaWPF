using ProyectoPeluqueria.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoPeluqueria.UserControlMenu
{
    /// <summary>
    /// Lógica de interacción para UserControlAgenda.xaml
    /// </summary>
    public partial class UserControlAgenda : UserControl
    {
        UserControlAgendaVM _ua;
        public UserControlAgenda()
        {
            _ua = new UserControlAgendaVM();
            InitializeComponent();
            DataContext = _ua;
        }

        private void CancelCita_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ua.OnCancelCita();
        }

        private void AddFechasSeleccionadas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ua.ModificaFechasMuestraCitas();
        }

        private void CancelCita_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _ua.CanCancelCita();
        }

    }
}
