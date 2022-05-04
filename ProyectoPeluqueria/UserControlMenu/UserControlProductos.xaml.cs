using ProyectoPeluqueria.Viewmodels;
using ProyectoPeluqueria.UserControlMenu;
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
using ProyectoPeluqueria.Modelos;

namespace ProyectoPeluqueria.UserControlMenu
{
    /// <summary>
    /// Lógica de interacción para UserControlProductosPelo.xaml
    /// </summary>
    public partial class UserControlProductos : UserControl
    {
        UserControlProductosVM _upe;

        public UserControlProductos()
        {
            _upe = new UserControlProductosVM();
            InitializeComponent();
            DataContext = _upe;
        }

        private void AddProductoCita_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _upe.OnShowAddProductoCitaForm();
        }

        private void AddProductoCita_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _upe.CanAddProductoCita();
        }

        private void ModificarProducto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _upe.OnModifyProductos();
        }

        private void ModificarProducto_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _upe.CanModifyProductos();
        }

        private void AddProducto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _upe.OnShowAddForm();
        }

        private void DelProducto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _upe.OnShowDelProducto();
        }

        private void DelProducto_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _upe.CanDeleteProductos();
        }

        private void AbreDialogoFoto_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _upe.OnOpenDialog();
        }

        

    }
}
