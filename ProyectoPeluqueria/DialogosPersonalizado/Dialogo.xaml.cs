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
using System.Windows.Shapes;

namespace ProyectoPeluqueria.DialogoPersonalizado
{
    /// <summary>
    /// Lógica de interacción para Dialogo.xaml
    /// </summary>
    public partial class Dialogo : Window
    {        
        public Dialogo()
        {
            InitializeComponent();            
        }

        private void Dialogo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }


    }
}
