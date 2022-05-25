using Newtonsoft.Json;
using ProyectoPeluqueria.DialogoPersonalizado;
using ProyectoPeluqueria.Modelos;
using RestSharp;
using System;
using System.Collections.ObjectModel;
using System.Net;

namespace ProyectoPeluqueria
{
    /// <summary>
    /// Clase que define los métodos necesarios para usar el servicio de API Rest
    /// </summary>
    public static class ServicioApiRest
    {
        /// <summary>
        /// Propiedad Cliente de tipo RestClient necesaria para el Request a la APIRest
        /// </summary>
        public static RestClient Client { get; set; }

        /// <summary>
        /// Propiedad Cook de tipo Cookie que se debe añadir al Request a la APIRest
        /// </summary>
        public static Cookie Cook { get; set; }


        /// <summary>
        /// Obtiene la cookie necesaría para las peticiones a la APIRest
        /// </summary>
        private static void GetCookie()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.api);
                request.CookieContainer = new CookieContainer();

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    Cook = response.Cookies[0];
                }
            }
            catch (WebException)
            {
                MuestraDialogo("No es posible conectarse al servidor");
            }
        }

        /// <summary>
        /// Muestra un diálogo informativo con un mensaje pasado por parámetro
        /// </summary>
        /// <param name="mensaje">string a mostrar</param>
        public static void MuestraDialogo(string mensaje)
        {
            Dialogo dialogo = new Dialogo();
            dialogo.MensajeText.Text = mensaje;
            dialogo.ShowDialog();
        }

        #region Login/Logout

        /// <summary>
        /// Este método se usa para la autentificación del usuario
        /// </summary>
        /// <param name="usuario">Objeto de tipo Usuario con los datos necesarios para el login</param>
        /// <returns>Un objeto de tipo MensajeLogin para evaluar el resultado de la petición. Si la cookie es nula, retorna
        /// un objeto vacío</returns>       
        public static MensajeLogin PostLogin(Usuario usuario)
        {
            Client = new RestClient(Properties.Settings.Default.api);
            GetCookie();

            if (Cook != null)
            {
                var request = new RestRequest("api/auth/login", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    username = usuario.Username,
                    password = usuario.Password
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeLogin>(response.Content);
            }
            else
            {
                return new MensajeLogin();
            }

        }

        /// <summary>
        /// Este método se usa para el logout del usuario
        /// </summary>
        /// <param name="usuario">Objeto de tipo Usuario con los datos necesarios para el logout</param>
        /// <returns>Un objeto de tipo MensajeLogout para evaluar el resultado de la petición</returns>
        public static MensajeLogout PostLogout()
        {
            try
            {
                var request = new RestRequest("api/auth/logout", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeLogout>(response.Content);
            }
            catch (Exception)
            {
                return new MensajeLogout("Error");
            }
        }

        #endregion

        #region Productos

        /// <summary>
        /// Este metodo se usa para obtener el listado de todos los grupos de productos.
        /// </summary>
        /// <returns>ObservableCollection de objetos ProductoGrupo</returns>
        public static ObservableCollection<ProductoGrupo> GetGrupos()
        {
            try
            {
                var request = new RestRequest("api/peluqueria/productosgrupos", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<ProductoGrupo>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<ProductoGrupo>();                
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de todos los productos en un grupo determinado
        /// </summary>
        /// <param name="grupo">nombre del grupo de productos</param>
        /// <returns>ObservableCollection de objetos Producto</returns>
        public static ObservableCollection<Producto> GetProductosGrupo(string grupo)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/productos/{grupo}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Producto>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Producto>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el id de un grupo de productos, mediante el nombre
        /// </summary>
        /// <param name="grupo">nombre del grupo</param>
        /// <returns>id del grupo</returns>
        public static int GetIdGrupo(string grupo)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/productos/grupo/{grupo}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;

                return int.Parse(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return 0;
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir un producto
        /// </summary>
        /// <param name="producto">objeto Producto que se quiere añadir</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PostProducto(Producto producto)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/producto", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    foto = producto.Foto,
                    nombre = producto.Nombre,
                    precio = producto.Precio,
                    idProductoGrupo = producto.IdProductoGrupo,
                    descripcion = producto.Descripcion,
                    stock = producto.Stock
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir un grupo de productos
        /// </summary>
        /// <param name="productoGrupo">objeto ProductoGrupo que se quiere añadir</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PostProductoGrupo(ProductoGrupo productoGrupo, string fotoBase64)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/productogrupo", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    foto = fotoBase64,
                    nombreGrupo = productoGrupo.NombreGrupo,
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para modificar un grupo de productos existente
        /// </summary>
        /// <param name="productoGrupo">objeto ProductoGrupo que se quiere modificar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutProductoGrupo(ProductoGrupo productoGrupo)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/productos/productogrupo", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    foto = productoGrupo.Foto,
                    idProductoGrupo = productoGrupo.IdProductoGrupo,
                    nombreGrupo = productoGrupo.NombreGrupo
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para modificar un producto existente
        /// </summary>
        /// <param name="producto">objeto Producto que se quiere modificar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutProducto(Producto producto)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/productos/producto", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    foto = producto.Foto,
                    idProducto = producto.IdProducto,
                    nombre = producto.Nombre,
                    precio = producto.Precio,
                    descripcion = producto.Descripcion,
                    stock = producto.Stock
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para eliminar un producto existente
        /// </summary>
        /// <param name="idProducto">id del producto que se quiere eliminar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral DelProducto(int idProducto)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/productos/producto/{idProducto}", Method.DELETE);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        #endregion

        #region Servicios

        /// <summary>
        /// Este metodo se usa para obtener el listado de todos los servicios
        /// </summary>
        /// <returns>ObservableCollection de objetos Servicio</returns>
        public static ObservableCollection<Servicio> GetServicios()
        {
            try
            {
                var request = new RestRequest("api/peluqueria/servicios", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;

                return JsonConvert.DeserializeObject<ObservableCollection<Servicio>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Servicio>();
            }
        }

        /// <summary>
        /// Este metodo se usa para modificar un servicio existente
        /// </summary>
        /// <param name="servicio">objeto Servicio que se quiere modificar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutServicio(Servicio servicio)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/servicios/servicio", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    foto = servicio.Foto,
                    idServicio = servicio.IdServicio,
                    nombre = servicio.Nombre,
                    precio = servicio.Precio
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para eliminar un servicio existente
        /// </summary>
        /// <param name="idServicio">id del Servicio que se quiere eliminar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral DelServicio(int idServicio)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/servicios/servicio/{idServicio}", Method.DELETE);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir un servicio
        /// </summary>
        /// <param name="servicio">objeto Servicio que se quiere añadir</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PostServicio(Servicio servicio)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/servicio", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    foto = servicio.Foto,
                    nombre = servicio.Nombre,
                    precio = servicio.Precio
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }


        #endregion

        #region Citas

        /// <summary>
        /// Este metodo se usa para obtener el listado completo de clientes
        /// </summary>
        /// <returns>ObservableCollection de usuarios cuyo rol es 'cliente'</returns>
        public static ObservableCollection<Usuario> GetClientes()
        {
            try
            {
                var request = new RestRequest("api/peluqueria/clientes", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Usuario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de fechas totalmente ocupadas, sin horarios libres para citas. El periodo engloba sesenta días.
        /// </summary>
        /// <param name="fechaComienzo">fecha de comienzo del periodo.</param>
        /// <returns>ObservableCollection de objetos DateTime</returns>
        public static ObservableCollection<DateTime> GetFechasOcupadas(DateTime fechaComienzo)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/fechas/ocupadas/{fechaComienzo:yyyy-MM-dd}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<DateTime>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<DateTime>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de citas para unos horarios, empleados y fecha o periodo de fechas.
        /// </summary>
        /// <param name="horarios">string con los id de los horarios separados por coma.</param>
        /// <param name="empleados">string con los id de cada empleado separados por coma.</param>
        /// <param name="fechaComienzo">fecha de comienzo del periodo.</param>
        /// <param name="fechaFin">fecha de fin del periodo.</param>
        /// <returns>ObservableCollection de objetos Cita</returns>
        public static ObservableCollection<Cita> GetCitasConfirmadasHorarioEmpleadoFecha(string horarios, string empleados,
            DateTime fechaComienzo, DateTime fechaFin)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/citas/buscador/{horarios}/{empleados}/{fechaComienzo:yyyy-MM-dd}/{fechaFin:yyyy-MM-dd}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Cita>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Cita>();
            }
        }

        /// <summary>
        /// Este método se usa para obtener la lista de citas para un usuario o todos los usuarios. Si se pasa un cero
        /// como parámetro de id de usuario, obtiene todas las citas.
        /// </summary>
        /// <param name="fechaComienzo">fecha de comienzo del periodo.</param>
        /// <param name="fechaFin">fecha de fin del periodo.</param>
        /// <param name="idUsuario">id del usuario del que se quieren obtener las citas, o cero si se quieren todas.</param>
        /// <returns>ObservableCollection de objetos Cita</returns>      
        public static ObservableCollection<Cita> GetCitas(DateTime fechaComienzo, DateTime fechaFin, int idUsuario)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/citas/{fechaComienzo:yyyy-MM-dd}/{fechaFin:yyyy-MM-dd}/{idUsuario}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Cita>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Cita>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de horarios libres en una fecha determinada, para la funcionalidad de citas.
        /// </summary>
        /// <param name="fecha">día en concreto para el que se quiere obtener el listado</param>
        /// <returns>ObservableCollection de objetos horario</returns>
        public static ObservableCollection<Horario> GetHorariosLibresFecha(DateTime fecha)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios/libres/{fecha:yyyy-MM-dd}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Horario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Horario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de horarios libres por empleado en una fecha, para la funcionalidad de citas.
        /// </summary>
        /// <param name="usuario">id del empleado para el que se quiere obtener el listado</param>
        /// <param name="fecha">día en concreto para el que se quiere obtener el listado</param>
        /// <returns>ObservableCollection de objetos horario</returns>
        public static ObservableCollection<Horario> GetHorariosLibresEmpleadosFecha(int usuario, DateTime fecha)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios/libres/empleados/{usuario}/{fecha:yyyy-MM-dd}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Horario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Horario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de empleados libres en una fecha y horario determinado
        /// </summary>
        /// <param name="idHorario">id del horario para el que se quiere obtener el listado</param>
        /// <param name="fecha">día en concreto para el que se quiere obtener el listado</param>
        /// <returns>ObservableCollection de usuarios de rol 'empleado'</returns>
        public static ObservableCollection<Usuario> GetEmpleadosDisponiblesOpcionHora(int idHorario, DateTime fecha)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/empleados/disponibles/{idHorario}/{fecha:yyyy-MM-dd}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Usuario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de empleados libres en una fecha determinada
        /// </summary>
        /// <param name="fecha">día en concreto para el que se quiere obtener el listado</param>
        /// <returns>ObservableCollection de usuarios de rol 'empleado'</returns>
        public static ObservableCollection<Usuario> GetEmpleadosDisponiblesOpcionProfesional(DateTime fecha)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/empleados/disponibles/fecha/{fecha:yyyy-MM-dd}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Usuario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de servicios que tiene un empleado determinado
        /// </summary>
        /// <param name="idEmpleado">id del empleado para el que se quiere obtener el listado</param>
        /// <returns>ObservableCollection de servicios</returns>
        public static ObservableCollection<Servicio> GetServiciosPorEmpleado(int idEmpleado)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/serviciosempleados/{idEmpleado}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;

                return JsonConvert.DeserializeObject<ObservableCollection<Servicio>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Servicio>();
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir una cita con su/s servicio/s asociado/s
        /// </summary>
        /// <param name="hora">id del horario seleccionado</param>
        /// <param name="empleado">id del empleado al que se le añade la cita</param>
        /// <param name="fecha">fecha de la cita</param>
        /// <param name="cliente">id del cliente al que se le añade la cita</param>
        /// <param name="servicios">string con los id de los servicios separados por coma</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PostCita(int hora, int empleado, DateTime fecha, int cliente, string servicios)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/cita/{hora}/{empleado}/{fecha:yyyy-MM-dd}/{cliente}/{servicios}", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir un producto a una cita
        /// </summary>
        /// <param name="idCita">id de la cita seleccionada</param>
        /// <param name="idProducto">id del producto a añadir</param>
        /// <param name="cantidadProducto">cantidad del producto a añadir</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PostProductoCita(int idCita, int idProducto, int cantidadProducto)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/cita/producto/{idCita}/{idProducto}/{cantidadProducto}", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para cancelar citas
        /// </summary>
        /// <param name="idCita">string con los id de las citas separados por coma.</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutCitaCancelada(string citas)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/citas/cancelar/{citas}", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        #endregion

        #region Usuarios

        /// <summary>
        /// Este metodo se usa para obtener el listado completo de empleados
        /// </summary>
        /// <returns>ObservableCollection de usuarios cuyo rol es 'empleado'</returns>
        public static ObservableCollection<Usuario> GetEmpleados()
        {
            try
            {
                var request = new RestRequest("api/peluqueria/empleados", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Usuario>();                
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de empleados que no tengan asignado un servicio determinado.
        /// </summary>
        /// <param name="idServicio">id del servicio que se quiere comprobar</param>
        /// <returns>ObservableCollection de empleados que no tengan ese servicio</returns>
        public static ObservableCollection<Usuario> GetEmpleadosFaltaServicios(int idServicio)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/empleados/servicio/{idServicio}", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Usuario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Usuario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para modificar datos de un usuario. No incluye cambios en: contraseña, rol, foto, fecha_alta, fecha_baja
        /// </summary>
        /// <param name="usuario">usuario de referencia a modificar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>      
        public static MensajeGeneral PutEmpleado(Usuario usuario)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/usuarios/usuario", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    apellidos = usuario.Apellidos,
                    foto = usuario.Foto,
                    email = usuario.Email,
                    telefono = usuario.Telefono,
                    idUsuario = usuario.IdUsuario,
                    nombre = usuario.Nombre,
                    username = usuario.Username
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir un servicio a un empleado
        /// </summary>
        /// <param name="idEmpleado">id del empleado al que se le añadirá el servicio</param>
        /// <param name="idServicio">id del servicio a añadir</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutEmpleadoServicio(int idEmpleado, int idServicio)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/empleados/servicios/{idEmpleado}/{idServicio}", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para modificar datos de un usuario, incluyendo su contraseña. No incluye cambios en: rol, foto, fecha_alta, fecha_baja
        /// </summary>
        /// <param name="usuario">usuario de referencia a modificar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>}
        public static MensajeGeneral PutEmpleadoPassw(Usuario usuario)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/usuarios/usuario/password", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    apellidos = usuario.Apellidos,
                    foto = usuario.Foto,
                    email = usuario.Email,
                    telefono = usuario.Telefono,
                    idUsuario = usuario.IdUsuario,
                    password = usuario.Password,
                    nombre = usuario.Nombre,
                    username = usuario.Username
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para modificar la fecha de baja de un usuario
        /// </summary>
        /// <param name="usuario">usuario de referencia a modificar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutEmpleadoFechaBaja(Usuario usuario)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/empleados/empleado/fechabaja", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    fecha_baja = usuario.Fecha_Baja.ToString("yyyy-MM-ddT00:00:00Z"),
                    idUsuario = usuario.IdUsuario
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir un nuevo usuario a la BBDD
        /// </summary>
        /// <param name="usuario">usuario a añadir</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PostUsuario(Usuario usuario, string fotoBase64)
        {
            try
            {
                var request = new RestRequest("api/peluqueria/usuario", Method.POST);

                request.AddCookie(Cook.Name, Cook.Value);

                request.AddJsonBody(new
                {
                    apellidos = usuario.Apellidos,
                    foto = fotoBase64,
                    fecha_alta = usuario.Fecha_Alta.ToString("yyyy-MM-ddT00:00:00Z"),
                    rol = Roles.EMPLEADO.ToString(),
                    password = usuario.Password,
                    nombre = usuario.Nombre,
                    username = usuario.Username,
                    email = usuario.Email,
                    telefono = usuario.Telefono
                });

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir los horarios a cada empleado
        /// </summary>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutEmpleadoHorarios()
        {
            try
            {
                var request = new RestRequest("api/peluqueria/empleados/empleado/horarios", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        #endregion

        #region Horarios

        /// <summary>
        /// Este metodo se usa para obtener el listado de no disponibilidad
        /// </summary>
        /// <returns>ObservableCollection de objetos Disponibilidad</returns>
        public static ObservableCollection<Disponibilidad> GetHorariosDeshabilitados()
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios/listanodisponibilidad", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Disponibilidad>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Disponibilidad>();
            }
        }

        /// <summary>
        /// Este metodo se usa para obtener el listado de horarios
        /// </summary>
        /// <returns>ObservableCollection de objetos Horario</returns>
        public static ObservableCollection<Horario> GetHorariosLista()
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios", Method.GET);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                string content = response.Content;


                return JsonConvert.DeserializeObject<ObservableCollection<Horario>>(content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new ObservableCollection<Horario>();
            }
        }

        /// <summary>
        /// Este metodo se usa para añadir el/los horario/s del/los empleado/s al listado de no disponibilidad, para una fecha o un periodo de fechas
        /// </summary>
        /// <param name="fechaComienzo">fecha de comienzo del periodo</param>
        /// <param name="fechaFin">fecha de fin del periodo</param>
        /// <param name="empleados">string con los id de cada empleado separados por coma</param>
        /// <param name="horas">string con los id de los horarios de cada empleado separados por coma</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutAddDisponibilidad(DateTime fechaComienzo, DateTime fechaFin, string empleados, string horas)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios/adddisponibilidad/{fechaComienzo:yyyy-MM-dd}/{fechaFin:yyyy-MM-dd}/{empleados}/{horas}", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para eliminar el/los horario/s del/los empleado/s del listado de no disponibilidad, para una fecha o un periodo de fechas
        /// </summary>
        /// <param name="fechaComienzo">fecha de comienzo del periodo</param>
        /// <param name="fechaFin">fecha de fin del periodo</param>
        /// <param name="empleados">string con los id de cada empleado separados por coma</param>
        /// <param name="horas">string con los id de los horarios de cada empleado separados por coma</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutDelDisponibilidad(DateTime fechaComienzo, DateTime fechaFin, string empleados, string horas)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios/deldisponibilidad/{fechaComienzo:yyyy-MM-dd}/{fechaFin:yyyy-MM-dd}/{empleados}/{horas}", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }

        /// <summary>
        /// Este metodo se usa para eliminar el/los horario/s del/los empleado/s del listado de no disponibilidad, usando los ids de los registros.
        /// </summary>
        /// <param name="listaIdDisponibilidad">string con el listado de ids de los registros de no disponibilidad que se quieren eliminar</param>
        /// <returns>Objeto de tipo MensajeGeneral para evaluar el resultado de la petición</returns>
        public static MensajeGeneral PutDelDisponibilidadIds(string listaIdDisponibilidad)
        {
            try
            {
                var request = new RestRequest($"api/peluqueria/horarios/deldisponibilidad/ids/{listaIdDisponibilidad}", Method.PUT);

                request.AddCookie(Cook.Name, Cook.Value);

                var response = Client.Execute(request);

                return JsonConvert.DeserializeObject<MensajeGeneral>(response.Content);
            }
            catch (Exception)
            {
                Properties.Settings.Default.autorizado = false;
                MuestraDialogo("Debe volver a iniciar sesión");
                return new MensajeGeneral("Error de acceso a la base de datos");
            }
        }


        #endregion  
    }
}
