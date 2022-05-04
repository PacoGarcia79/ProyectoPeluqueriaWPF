using ProyectoPeluqueria.Modelos;
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
    /// Lógica de interacción para UserControlCitas.xaml
    /// </summary>
    public partial class UserControlCitas : UserControl
    {
        UserControlCitasVM _uc;

        public UserControlCitas()
        {
            _uc = new UserControlCitasVM();
            InitializeComponent();
            DataContext = _uc;
        }

        private void AddCita_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _uc.OnAdd();            
        }

        private void AddCita_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _uc.CanAdd();
        }
    }
}
