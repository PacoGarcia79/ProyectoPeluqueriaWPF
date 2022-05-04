using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Clase usada para poder seleccionar varios items en un listbox
    /// </summary>
    class CustomListBox : ListBox
    {
        public CustomListBox()
        {
            this.SelectionChanged += CustomListBox_SelectionChanged;
        }

        void CustomListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItemsList = this.SelectedItems;
        }
        
        /// <summary>
        /// Método que devuelve una lista de items seleccionados
        /// </summary>
        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomListBox), new PropertyMetadata(null));

    }
}
