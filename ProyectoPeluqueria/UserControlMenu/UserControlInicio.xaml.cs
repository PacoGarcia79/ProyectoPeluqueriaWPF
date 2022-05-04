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
    /// Interação lógica para UserControlHome.xam
    /// </summary>
    public partial class UserControlInicio : UserControl
    {
        UserControlInicioVM _ui;
        public UserControlInicio()
        {
            _ui = new UserControlInicioVM();
            InitializeComponent();
            DataContext = _ui;
        }
    }
}
