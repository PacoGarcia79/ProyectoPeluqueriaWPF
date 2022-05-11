using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoPeluqueria.UserControlMenu.ComandosUserControl
{
    public static class CustomCommandsUserControl
    {

        public static readonly RoutedUICommand ModificarEmpleado = new RoutedUICommand
        (
            "ModificarEmpleado", "ModificarEmpleado",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand BajaEmpleado = new RoutedUICommand
        (
            "BajaEmpleado", "BajaEmpleado",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand AddEmpleado = new RoutedUICommand
        (
            "AddEmpleado", "AddEmpleado",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
               
            }
        );


        public static readonly RoutedUICommand ModificarServicio = new RoutedUICommand
        (
            "ModificarServicio", "ModificarServicio",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand AddServicio = new RoutedUICommand
        (
            "AddServicio", "AddServicio",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand AddServicioEmpleado = new RoutedUICommand
        (
            "AddServicioEmpleado", "AddServicioEmpleado",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );


        public static readonly RoutedUICommand DelServicio = new RoutedUICommand
        (
            "DelServicio", "DelServicio",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );


        public static readonly RoutedUICommand ModificarProducto = new RoutedUICommand
       (
           "ModificarProducto", "ModificarProducto",
           typeof(CustomCommandsUserControl),
           new InputGestureCollection()
           {
                
           }
       );

        public static readonly RoutedUICommand AddProducto = new RoutedUICommand
      (
          "AddProducto", "AddProducto",
          typeof(CustomCommandsUserControl),
          new InputGestureCollection()
          {
              
          }
      );

        public static readonly RoutedUICommand AddProductoGrupo = new RoutedUICommand
      (
          "AddProductoGrupo", "AddProductoGrupo",
          typeof(CustomCommandsUserControl),
          new InputGestureCollection()
          {
             
          }
      );

        public static readonly RoutedUICommand ModifyProductoGrupo = new RoutedUICommand
        (
            "ModifyProductoGrupo", "ModifyProductoGrupo",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
               
            }
        );

        public static readonly RoutedUICommand DelProducto = new RoutedUICommand
      (
          "DelProducto", "DelProducto",
          typeof(CustomCommandsUserControl),
          new InputGestureCollection()
          {
             
          }
      );

        public static readonly RoutedUICommand AddCita = new RoutedUICommand
        (
            "AddProductoCita", "AddProductoCita",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
               
            }
        );

        public static readonly RoutedUICommand AddFechasSeleccionadas = new RoutedUICommand
        (
            "AddFechasSeleccionadas", "AddFechasSeleccionadas",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand CancelCita = new RoutedUICommand
        (
            "CancelCita", "CancelCita",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand AddProductoCita = new RoutedUICommand
        (
            "AddProductoCita", "AddProductoCita",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand HabilitaDisponibilidad = new RoutedUICommand
        (
            "HabilitaDisponibilidad", "HabilitaDisponibilidad",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand DeshabilitaDisponibilidad = new RoutedUICommand
        (
            "DeshabilitaDisponibilidad", "DeshabilitaDisponibilidad",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );

        public static readonly RoutedUICommand AbreDialogoFoto = new RoutedUICommand
        (
            "AbreDialogoFoto", "AbreDialogoFoto",
            typeof(CustomCommandsUserControl),
            new InputGestureCollection()
            {
                
            }
        );





    }
}
