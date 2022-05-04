using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace ProyectoPeluqueria.AttachedProperties
{
    /// <summary>
    /// Clase usada para implementar la selección múltiple en un DataGrid 
    /// Comportamiento de sincronización para un multiselector.
    /// <href>https://github.com/itsChris/WpfMvvmDataGridMultiselect</href>
    /// </summary>
    public static class MultiSelectorBehaviours
    {
        public static readonly DependencyProperty SynchronizedSelectedItems = DependencyProperty.RegisterAttached(
            "SynchronizedSelectedItems", typeof(IList), typeof(MultiSelectorBehaviours), new PropertyMetadata(null, OnSynchronizedSelectedItemsChanged));

        private static readonly DependencyProperty SynchronizationManagerProperty = DependencyProperty.RegisterAttached(
            "SynchronizationManager", typeof(SynchronizationManager), typeof(MultiSelectorBehaviours), new PropertyMetadata(null));

        /// <summary>
        /// Obtiene los elementos seleccionados sincronizados.
        /// </summary>
        /// <param name="dependencyObject">Objeto de dependencia.</param>
        /// <returns>Lista que actúa como la lista de sincronización.</returns>
        public static IList GetSynchronizedSelectedItems(DependencyObject dependencyObject)
        {
            return (IList)dependencyObject.GetValue(SynchronizedSelectedItems);
        }

        /// <summary>
        /// Establece los elementos seleccionados sincronizados.
        /// </summary>
        /// <param name="dependencyObject">Objeto de dependencia.</param>
        /// <param name="value">El valor que se establecerá como elementos sincronizados.</param>
        public static void SetSynchronizedSelectedItems(DependencyObject dependencyObject, IList value)
        {
            dependencyObject.SetValue(SynchronizedSelectedItems, value);
        }

        private static SynchronizationManager GetSynchronizationManager(DependencyObject dependencyObject)
        {
            return (SynchronizationManager)dependencyObject.GetValue(SynchronizationManagerProperty);
        }

        private static void SetSynchronizationManager(DependencyObject dependencyObject, SynchronizationManager value)
        {
            dependencyObject.SetValue(SynchronizationManagerProperty, value);
        }

        private static void OnSynchronizedSelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                SynchronizationManager synchronizer = GetSynchronizationManager(dependencyObject);
                synchronizer.StopSynchronizing();

                SetSynchronizationManager(dependencyObject, null);
            }

            IList list = e.NewValue as IList;
            Selector selector = dependencyObject as Selector;

            // verifica que esta propiedad es una IList y que se establece en un ListBox
            if (list != null && selector != null)
            {
                SynchronizationManager synchronizer = GetSynchronizationManager(dependencyObject);
                if (synchronizer == null)
                {
                    synchronizer = new SynchronizationManager(selector);
                    SetSynchronizationManager(dependencyObject, synchronizer);
                }

                synchronizer.StartSynchronizingList();
            }
        }
    }
}
