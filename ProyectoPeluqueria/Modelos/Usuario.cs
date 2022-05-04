using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPeluqueria.Modelos
{
    /// <summary>
    /// Entidad usada para la gestión de los usuarios que harán uso de la aplicación.
    /// </summary>
    public class Usuario : INotifyPropertyChanged
    {
        private int _idUsuario;
        public int IdUsuario
        {
            get { return _idUsuario; }
            set
            {
                if (_idUsuario != value)
                {
                    _idUsuario = value;
                    NotifyPropertyChanged("IdUsuario");
                }
            }
        }


        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                if (_nombre != value)
                {
                    _nombre = value;
                    NotifyPropertyChanged("Nombre");
                }
            }
        }

        private string _apellidos;
        public string Apellidos
        {
            get { return _apellidos; }
            set
            {
                if (_apellidos != value)
                {
                    _apellidos = value;
                    NotifyPropertyChanged("Apellidos");
                }
            }
        }


        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    NotifyPropertyChanged("Username");
                }
            }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    NotifyPropertyChanged("Password");
                }
            }
        }

        private DateTime _fecha_Alta;
        public DateTime Fecha_Alta
        {
            get { return _fecha_Alta; }
            set
            {
                if (_fecha_Alta != value)
                {
                    _fecha_Alta = value;
                    NotifyPropertyChanged("Fecha_Alta");
                }
            }
        }

        private DateTime _fecha_Baja;
        public DateTime Fecha_Baja
        {
            get { return _fecha_Baja; }
            set
            {
                if (_fecha_Baja != value)
                {
                    _fecha_Baja = value;
                    NotifyPropertyChanged("Fecha_Baja");
                }
            }
        }


        private Roles _rol;
        public Roles Rol
        {
            get { return _rol; }
            set
            {
                if (_rol != value)
                {
                    _rol = value;
                    NotifyPropertyChanged("Rol");
                }
            }
        }

        private string _foto;
        public string Foto
        {
            get { return _foto; }
            set
            {
                if (_foto != value)
                {
                    _foto = value;
                    NotifyPropertyChanged("Foto");
                }
            }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    NotifyPropertyChanged("Email");
                }
            }
        }

        private string _telefono;
        public string Telefono
        {
            get { return _telefono; }
            set
            {
                if (_telefono != value)
                {
                    _telefono = value;
                    NotifyPropertyChanged("Telefono");
                }
            }
        }


        private string _confirmaContraseña;
        public string ConfirmaContraseña
        {
            get { return _confirmaContraseña; }
            set
            {
                if (_confirmaContraseña != value)
                {
                    _confirmaContraseña = value;
                    NotifyPropertyChanged("ConfirmaContraseña");
                }
            }
        }

        private bool _modificaContraseña;
        public bool ModificaContraseña
        {
            get { return _modificaContraseña; }
            set
            {
                if (_modificaContraseña != value)
                {
                    _modificaContraseña = value;
                    NotifyPropertyChanged("ModificaContraseña");
                }
            }
        }

        /// <summary>
        /// Ruta de la foto del usuario que se va a modificar
        /// </summary>
        private string _rutaFotoNueva;
        public string RutaFotoNueva
        {
            get { return _rutaFotoNueva; }
            set
            {
                if (_rutaFotoNueva != value)
                {
                    _rutaFotoNueva = value;
                    NotifyPropertyChanged("RutaFotoNueva");
                }
            }
        }

        public Usuario()
        {
        }

        public Usuario(int idUsuario, string nombre)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
        }

        public Usuario(int idUsuario, string nombre, string apellidos, string username, string password, DateTime fechaAlta, DateTime fechaBaja, Roles rol, string foto, string email, string telefono)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Username = username;
            Password = password;
            Fecha_Alta = fechaAlta;
            Fecha_Baja = fechaBaja;
            Rol = rol;
            Foto = foto;
            Email = email;
            Telefono = telefono;
        }

        public Usuario(int idUsuario, string nombre, string apellidos, string username, string password, DateTime fecha_Alta, DateTime fecha_Baja, string foto)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Username = username;
            Password = password;
            Fecha_Alta = fecha_Alta;
            Fecha_Baja = fecha_Baja;
            Foto = foto;
        }

        public Usuario(int idUsuario, string nombre, string apellidos, string username, string foto)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Username = username;
            Foto = foto;
        }



        public Usuario(int idUsuario, string nombre, string apellidos, string username, string foto, string email, string telefono)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Username = username;
            Foto = foto;
            Email = email;
            Telefono = telefono;
        }

        public Usuario(int idUsuario, string nombre, string apellidos, string username, string email, string telefono)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Username = username;
            Email = email;
            Telefono = telefono;
        }

        public Usuario(int idUsuario, string nombre, string apellidos, string username, string password, string foto, string email, string telefono)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Username = username;
            Password = password;
            Foto = foto;
            Email = email;
            Telefono = telefono;
        }

        public Usuario(int idUsuario, string nombre, string apellidos, string foto)
        {
            IdUsuario = idUsuario;
            Nombre = nombre;
            Apellidos = apellidos;
            Foto = foto;
        }

        public Usuario(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string ToJson()
        {
            string cadena = "{'username': '" + Username + "','password': '" + Password + "'}";
            return cadena.Replace("'", "\"");
        }

        public override string ToString()
        {
            return $"{IdUsuario}";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

}

    