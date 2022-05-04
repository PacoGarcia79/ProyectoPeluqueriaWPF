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
   
    public partial class UserControlProductosGrupos : UserControl
    {
        UserControlProductosGruposVM _up;

        public UserControlProductosGrupos()
        {
            _up = new UserControlProductosGruposVM();
            InitializeComponent();
            DataContext = _up;
        }

        private void AddProductoGrupo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _up.OnShowAddForm();
        }


        private void ModifyProductoGrupo_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _up.OnShowModifyForm();
        }


    }
}
