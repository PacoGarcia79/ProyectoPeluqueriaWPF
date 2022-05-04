using Microsoft.Expression.Interactivity.Core;
using ProyectoPeluqueria.Modelos;
using ProyectoPeluqueria.UserControlMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoPeluqueria.Viewmodels
{
    /// <summary>
    /// Vista modelo del UserControl Productos Main
    /// </summary>
    class UserControlProductosMainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// UserControl seleccionado
        /// </summary>
        private UserControl _selectedUserControl;
        public UserControl SelectedUserControl
        {
            get { return _selectedUserControl; }
            set
            {
                if (_selectedUserControl != value)
                {
                    _selectedUserControl = value;
                    NotifyPropertyChanged("SelectedUserControl");
                }
            }
        }

        /// <summary>
        /// ICommand para la navegación. Se usa para navegar a la pantalla de listado de productos.
        /// </summary>
        public ICommand DisplayProductos
        {
            get
            {
                return new ActionCommand(action => SelectedUserControl = new UserControlProductos());
            }
        }

        /// <summary>
        /// ICommand para la navegación. Se usa para navegar a la pantalla de listado de grupos de productos.
        /// </summary>
        public ICommand DisplayMainProductos
        {
            get
            {
                return new ActionCommand(action => SelectedUserControl = new UserControlProductosGrupos());
            }
        }

        public UserControlProductosMainVM()
        {
            SelectedUserControl = new UserControlProductosGrupos();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
