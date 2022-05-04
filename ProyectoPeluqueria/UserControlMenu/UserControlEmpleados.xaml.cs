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
    /// Lógica de interacción para UserControlEmpleados.xaml
    /// </summary>
    public partial class UserControlEmpleados : UserControl
    {
        UserControlEmpleadosVM _ue;
        public UserControlEmpleados()
        {
            _ue = new UserControlEmpleadosVM();
            InitializeComponent();
            DataContext = _ue;
        }

        private void ModificarEmpleado_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ue.OnModifyEmpleado();
        }

        private void ModificarEmpleado_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _ue.CanModifyEmpleado();
        }

        private void AddEmpleado_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ue.OnShowAddForm();
        }

        private void BajaEmpleado_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ue.OnShowBajaEmpleado();
        }

        private void BajaEmpleado_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _ue.CanBajaEmpleado();
        }

        private void AbreDialogoFoto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _ue.OnOpenDialog();
        }

    }
}
