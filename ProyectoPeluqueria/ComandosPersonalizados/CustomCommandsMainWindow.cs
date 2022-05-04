using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoPeluqueria.Comandos
{
    public static class CustomCommands
    {

        public static readonly RoutedUICommand Salir = new RoutedUICommand
        (
            "Salir", "Salir",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand ElementoVentana = new RoutedUICommand
       (
           "ElementoVentana", "ElementoVentana",
           typeof(CustomCommands),
           new InputGestureCollection()
           {
                new KeyGesture(Key.E, ModifierKeys.Control)
           }
       );

        public static readonly RoutedUICommand LoginForm = new RoutedUICommand
      (
          "LoginForm", "LoginForm",
          typeof(CustomCommands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.L, ModifierKeys.Control)
          }
      );

        public static readonly RoutedUICommand Logout = new RoutedUICommand
    (
        "Logout", "Logout",
        typeof(CustomCommands),
        new InputGestureCollection()
        {
                new KeyGesture(Key.U, ModifierKeys.Control)
        }
    );

        public static readonly RoutedUICommand AbrirDrawer = new RoutedUICommand(
            "AbrirDrawer", "AbrirDrawer",
            typeof(CustomCommands),
            new InputGestureCollection
            { new KeyGesture(Key.O,ModifierKeys.Control)}
            );

        public static readonly RoutedUICommand CerrarDrawer = new RoutedUICommand(
            "CerrarDrawer", "CerrarDrawer",
            typeof(CustomCommands),
            new InputGestureCollection
            { new KeyGesture(Key.C,ModifierKeys.Control)}
            );


    }
}
