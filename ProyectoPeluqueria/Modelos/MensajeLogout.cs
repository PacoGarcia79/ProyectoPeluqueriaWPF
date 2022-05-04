using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de la respuesta de la APIRest en lo referente al logout del usuario
    /// </summary>
    public class MensajeLogout : INotifyPropertyChanged
    {
        public string Id_Session { get; set; }
        public string Usuario { get; set; }

        public MensajeLogout()
        {
            Id_Session = "";
            Usuario = "";
        }

        public MensajeLogout(string id_Session, string usuario)
        {
            Id_Session = id_Session;
            Usuario = usuario;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
