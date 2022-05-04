using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de la respuesta de la APIRest en lo referente al login del usuario
    /// </summary>
    public class MensajeLogin : INotifyPropertyChanged
    {
        public string Id_Session { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Mensaje { get; set; }

        public MensajeLogin()
        {
            Id_Session = "";
            IdUsuario = 0;
            Usuario = "";
            Nombre = "";
            Rol = "";
            Mensaje = "";
        }

        public MensajeLogin(string id_Session, string usuario, string nombre, string rol, string mensaje)
        {
            Id_Session = id_Session;
            Usuario = usuario;
            Nombre = nombre;
            Rol = rol;
            Mensaje = mensaje;
        }

        public MensajeLogin(string id_Session, int idUsuario, string usuario, string nombre, string rol, string mensaje)
        {
            Id_Session = id_Session;
            IdUsuario = idUsuario;
            Usuario = usuario;
            Nombre = nombre;
            Rol = rol;
            Mensaje = mensaje;
        }

        public override string ToString()
        {
            return $"{Usuario} / {Nombre} / {Rol}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}


