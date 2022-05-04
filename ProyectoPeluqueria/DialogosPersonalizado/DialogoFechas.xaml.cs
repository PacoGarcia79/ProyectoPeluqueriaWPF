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
    /// Lógica de interacción para DialogoFechas.xaml
    /// </summary>
    public partial class DialogoFechas : Window
    {
        public DateTime[] Fechas { get; set; }

        public List<DateTime> FechasSeleccionadas { get; set; }

        public DialogoFechas()
        {            
            InitializeComponent();
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.Calendar cal = (System.Windows.Controls.Calendar)sender;

            Fechas = cal.SelectedDates.ToArray();            
            FechasSeleccionadas = new List<DateTime>(Fechas);
        }

        private void Dialogo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
