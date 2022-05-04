using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ProyectoPeluqueria.AttachedProperties
{
    /// <summary>
    /// Clase usada para implementar la selección múltiple en un DataGrid 
    /// 
    /// <href>https://github.com/itsChris/WpfMvvmDataGridMultiselect</href>
    /// </summary>
    public class SynchronizationManager
    {
        private readonly Selector _multiSelector;
        private TwoListSynchronizer _synchronizer;

        /// <summary>
        /// Constructor de la clase <see cref="SynchronizationManager"/>.
        /// </summary>
        /// <param name="selector">Selector.</param>
        internal SynchronizationManager(Selector selector)
        {
            _multiSelector = selector;
        }

        /// <summary>
        /// Comienza la sincronización las listas.
        /// </summary>
        public void StartSynchronizingList()
        {
            IList list = MultiSelectorBehaviours.GetSynchronizedSelectedItems(_multiSelector);

            if (list != null)
            {
                _synchronizer = new TwoListSynchronizer(GetSelectedItemsCollection(_multiSelector), list);
                _synchronizer.StartSynchronizing();
            }
        }

        /// <summary>
        /// Finaliza la sincronización las listas.
        /// </summary>
        public void StopSynchronizing()
        {
            _synchronizer.StopSynchronizing();
        }

        public static IList GetSelectedItemsCollection(Selector selector)
        {
            if (selector is MultiSelector)
            {
                return (selector as MultiSelector).SelectedItems;
            }
            else if (selector is ListBox)
            {
                return (selector as ListBox).SelectedItems;
            }
            else
            {
                throw new InvalidOperationException("Target object has no SelectedItems property to bind.");
            }
        }
    }
}