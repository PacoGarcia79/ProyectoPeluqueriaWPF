using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de las diferentes elementos del menú drawer 
    /// </summary>
    class ElementoMenu : INotifyPropertyChanged
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Contenido { get; set; }

        public ElementoMenu(string nombre, string tipo, string contenido)
        {
            Nombre = nombre;
            Tipo = tipo;
            Contenido = contenido;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
