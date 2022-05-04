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
    /// Lógica de interacción para UserControlProductosMain.xaml
    /// </summary>
    public partial class UserControlProductosMain : UserControl
    {

        UserControlProductosMainVM _up;

        public UserControlProductosMain()
        {
            _up = new UserControlProductosMainVM();
            InitializeComponent();
            DataContext = _up;
        }

    }
}
