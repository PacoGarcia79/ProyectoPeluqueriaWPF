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
    /// Lógica de interacción para UserControlServicios.xaml
    /// </summary>
    public partial class UserControlServicios : UserControl
    {
        UserControlServiciosVM _us;
        public UserControlServicios()
        {
            _us = new UserControlServiciosVM();
            InitializeComponent();
            DataContext = _us;
        }

        private void ModificarServicio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _us.OnModifyServicio();
        }

        private void ModificarServicio_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _us.CanModifyServicio();
        }

        private void AddServicio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _us.OnShowAddForm();
        }

        private void DelServicio_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _us.OnShowDelServicio();
        }

        private void AddServicioEmpleado_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _us.OnShowAddServicioEmpleadoForm();
        }

        private void AddServicioEmpleado_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _us.CanDeleteServiciosAddServicioEmpleado();
        }


        private void AbreDialogoFoto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _us.OnOpenDialog();
        }

        private void DelServicio_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _us.CanDeleteServiciosAddServicioEmpleado();
        }

    }
}
