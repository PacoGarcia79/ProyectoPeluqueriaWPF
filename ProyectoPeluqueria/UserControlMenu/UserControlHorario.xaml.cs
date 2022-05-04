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
    /// Lógica de interacción para UserControlHorario.xaml
    /// </summary>
    public partial class UserControlHorario : UserControl
    {
        UserControlHorarioVM _uh;

        public UserControlHorario()
        {
            _uh = new UserControlHorarioVM();
            InitializeComponent();
            DataContext = _uh;
        }

        private void HabilitaDisponibilidad_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _uh.HabilitaDisponibilidad();
        }

        private void HabilitaDisponibilidad_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _uh.CanHabilitarDisponibilidad();
        }

        private void ModificarDisponibilidad_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _uh.CanModifiyDisponibilidad();
        }

        private void DeshabilitaDisponibilidad_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _uh.DeshabilitaDisponibilidad();
        }

        private void AddFechasSeleccionadas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _uh.ModificaFechasSeleccionHorarios();
        }


    }
}
