using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de la respuesta de la APIRest
    /// </summary>
    public class MensajeGeneral : INotifyPropertyChanged
    {
        public string Mensaje { get; set; }

        public MensajeGeneral()
        {
            Mensaje = "";
        }

        public MensajeGeneral(string contenido)
        {
            Mensaje = contenido;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
