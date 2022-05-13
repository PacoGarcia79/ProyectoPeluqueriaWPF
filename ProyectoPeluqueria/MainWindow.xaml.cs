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

namespace ProyectoPeluqueria
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM _vm;

        public MainWindow()
        {
            _vm = new MainWindowVM();
            InitializeComponent();
            DataContext = _vm;
        }

        private void Logout_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.OnShowLogout();
        }

        private void LoginForm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.OnShowLoginForm();
        }

        private void LoginForm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.CanLoginForm();
        }


        private void ElementoVentana_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.UpdateUserControl();
        }

        private void ElementoVentana_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _vm.CanUpdateUserControl();
        }

        private void Salir_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _vm.Salir();
        }

    }
}
