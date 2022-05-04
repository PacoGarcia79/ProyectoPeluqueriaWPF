using ProyectoPeluqueria.ConverterUserControl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProyectoPeluqueria.AttachedProperties
{
    /// <summary>
    /// Clase usada para implementar la selección múltiple en un DataGrid
    /// Se encarga de sincronizar las listas principal y destino
    /// 
    /// <href>https://github.com/itsChris/WpfMvvmDataGridMultiselect</href>
    /// </summary>
    public class TwoListSynchronizer : IWeakEventListener
    {
        private static readonly IListItemConverter DefaultConverter = new DoNothingListItemConverter();
        private readonly IList _masterList;
        private readonly IListItemConverter _masterTargetConverter;
        private readonly IList _targetList;
        /// <summary>
        /// Constructor de la clase <see cref="TwoListSynchronizer"/>.
        /// </summary>
        /// <param name="masterList">Lista principal.</param>
        /// <param name="targetList">Lista destino.</param>
        /// <param name="masterTargetConverter">Conversor Principal-Destino.</param>
        public TwoListSynchronizer(IList masterList, IList targetList, IListItemConverter masterTargetConverter)
        {
            _masterList = masterList;
            _targetList = targetList;
            _masterTargetConverter = masterTargetConverter;
        }
        /// <summary>
        /// Constructor de la clase <see cref="TwoListSynchronizer"/>.
        /// </summary>
        /// <param name="masterList">Lista principal.</param>
        /// <param name="targetList">Lista destino.</param>
        public TwoListSynchronizer(IList masterList, IList targetList)
            : this(masterList, targetList, DefaultConverter)
        {
        }
        private delegate void ChangeListAction(IList list, NotifyCollectionChangedEventArgs e, Converter<object, object> converter);
        /// <summary>
        /// Comienza la sincronización las listas.
        /// </summary>
        public void StartSynchronizing()
        {
            ListenForChangeEvents(_masterList);
            ListenForChangeEvents(_targetList);

            // Actualiza la lista destino desde la lista principal
            SetListValuesFromSource(_masterList, _targetList, ConvertFromMasterToTarget);

            // En algunos casos, la lista destino puede tener su propia vista en la cual los elementos deben incluirse:
            // actualiza la lista principal desde la lista destino
            // (Este es el caso con una colección ListBox SelectedItems: solo los elementos de ItemsSource pueden incluirse en SelectedItems)
            if (!TargetAndMasterCollectionsAreEqual())
            {
                SetListValuesFromSource(_targetList, _masterList, ConvertFromTargetToMaster);
            }
        }
        /// <summary>
        /// Finaliza la sincronización las listas.
        /// </summary>
        public void StopSynchronizing()
        {
            StopListeningForChangeEvents(_masterList);
            StopListeningForChangeEvents(_targetList);
        }
        /// <summary>
        ///Recibe eventos del administrador de eventos centralizado.
        /// </summary>
        /// <param name="managerType">El tipo de <see cref="T:System.Windows.WeakEventManager"/> llamando a este método.</param>
        /// <param name="sender">Objecto que origina el evento.</param>
        /// <param name="e">Datos del evento.</param>
        /// <returns>
        /// true si el listener manejó el evento. Se considera un error por parte del <see cref="T:System.Windows.WeakEventManager"/> handling en WPF para registrar un listener para un evento que el listener no controla. De todos modos, el método debería devolver falso si recibe un evento que no reconoce ni maneja.
        /// </returns>                                                                                          
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            HandleCollectionChanged(sender as IList, e as NotifyCollectionChangedEventArgs);

            return true;
        }
        /// <summary>
        /// Escucha eventos de cambio en una lista.
        /// </summary>
        /// <param name="list">La lista a escuchar.</param>
        protected void ListenForChangeEvents(IList list)
        {
            if (list is INotifyCollectionChanged)
            {
                CollectionChangedEventManager.AddListener(list as INotifyCollectionChanged, this);
            }
        }
        /// <summary>
        /// Finaliza la escucha de eventos de cambio en una lista.
        /// </summary>
        /// <param name="list">La lista que se deja de escuchar.</param>
        protected void StopListeningForChangeEvents(IList list)
        {
            if (list is INotifyCollectionChanged)
            {
                CollectionChangedEventManager.RemoveListener(list as INotifyCollectionChanged, this);
            }
        }
        private void AddItems(IList list, NotifyCollectionChangedEventArgs e, Converter<object, object> converter)
        {
            int itemCount = e.NewItems.Count;

            for (int i = 0; i < itemCount; i++)
            {
                int insertionPoint = e.NewStartingIndex + i;

                if (insertionPoint > list.Count)
                {
                    list.Add(converter(e.NewItems[i]));
                }
                else
                {
                    list.Insert(insertionPoint, converter(e.NewItems[i]));
                }
            }
        }
        private object ConvertFromMasterToTarget(object masterListItem)
        {
            return _masterTargetConverter == null ? masterListItem : _masterTargetConverter.Convert(masterListItem);
        }
        private object ConvertFromTargetToMaster(object targetListItem)
        {
            return _masterTargetConverter == null ? targetListItem : _masterTargetConverter.ConvertBack(targetListItem);
        }
        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IList sourceList = sender as IList;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    PerformActionOnAllLists(AddItems, sourceList, e);
                    break;
                case NotifyCollectionChangedAction.Move:
                    PerformActionOnAllLists(MoveItems, sourceList, e);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    PerformActionOnAllLists(RemoveItems, sourceList, e);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    PerformActionOnAllLists(ReplaceItems, sourceList, e);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    UpdateListsFromSource(sender as IList);
                    break;
                default:
                    break;
            }
        }
        private void MoveItems(IList list, NotifyCollectionChangedEventArgs e, Converter<object, object> converter)
        {
            RemoveItems(list, e, converter);
            AddItems(list, e, converter);
        }
        private void PerformActionOnAllLists(ChangeListAction action, IList sourceList, NotifyCollectionChangedEventArgs collectionChangedArgs)
        {
            if (sourceList == _masterList)
            {
                PerformActionOnList(_targetList, action, collectionChangedArgs, ConvertFromMasterToTarget);
            }
            else
            {
                PerformActionOnList(_masterList, action, collectionChangedArgs, ConvertFromTargetToMaster);
            }
        }
        private void PerformActionOnList(IList list, ChangeListAction action, NotifyCollectionChangedEventArgs collectionChangedArgs, Converter<object, object> converter)
        {
            StopListeningForChangeEvents(list);
            action(list, collectionChangedArgs, converter);
            ListenForChangeEvents(list);
        }
        private void RemoveItems(IList list, NotifyCollectionChangedEventArgs e, Converter<object, object> converter)
        {
            int itemCount = e.OldItems.Count;

            // para la cantidad de elementos que se eliminan, elimina el elemento del índice de inicio anterior
            // (esto hará que los siguientes elementos se desplacen hacia abajo para llenar el hueco).
            for (int i = 0; i < itemCount; i++)
            {
                list.RemoveAt(e.OldStartingIndex);
            }
        }
        private void ReplaceItems(IList list, NotifyCollectionChangedEventArgs e, Converter<object, object> converter)
        {
            RemoveItems(list, e, converter);
            AddItems(list, e, converter);
        }
        private void SetListValuesFromSource(IList sourceList, IList targetList, Converter<object, object> converter)
        {
            StopListeningForChangeEvents(targetList);

            targetList.Clear();

            foreach (object o in sourceList)
            {
                targetList.Add(converter(o));
            }

            ListenForChangeEvents(targetList);
        }
        private bool TargetAndMasterCollectionsAreEqual()
        {
            return _masterList.Cast<object>().SequenceEqual(_targetList.Cast<object>().Select(item => ConvertFromTargetToMaster(item)));
        }
        /// <summary>
        /// Se asegura de que todas las listas sincronizadas tengan los mismos valores que la lista de origen.
        /// </summary>
        /// <param name="sourceList">Lista de origen.</param>
        private void UpdateListsFromSource(IList sourceList)
        {
            if (sourceList == _masterList)
            {
                SetListValuesFromSource(_masterList, _targetList, ConvertFromMasterToTarget);
            }
            else
            {
                SetListValuesFromSource(_targetList, _masterList, ConvertFromTargetToMaster);
            }
        }
        /// <summary>
        /// Una implementación que no hace nada en las conversiones.
        /// </summary>
        internal class DoNothingListItemConverter : IListItemConverter
        {
            /// <summary>
            /// Convierte el elemento especificado de la lista principal.
            /// </summary>
            /// <param name="masterListItem">El elemento especificado de la lista principal.</param>
            /// <returns>Resultado de la conversión.</returns>
            public object Convert(object masterListItem)
            {
                return masterListItem;
            }

            /// <summary>
            /// Convierte el elemento especificado de la lista de destino.
            /// </summary>
            /// <param name="targetListItem">El elemento especificado de la lista de destino.</param>
            /// <returns>Resultado de la conversión.</returns>
            public object ConvertBack(object targetListItem)
            {
                return targetListItem;
            }
        }
    }
}
