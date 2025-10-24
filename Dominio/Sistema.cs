using Dominio.Enums;
using System.Net.Http;

namespace Dominio
{
    public class Sistema
    {
        private static Sistema s_instancia;

        private List<Usuario> _listaUsuarios = new List<Usuario>();
        private List<Articulo> _listaArticulos = new List<Articulo>();
        private List<Publicacion> _listaPublicaciones = new List<Publicacion>();
       

        private Sistema()
        {
            PrecargarUsuarios();
            PrecargarArticulos();
            PrecargarPublicaciones();
        }

        public static Sistema Instancia
        {
            get
            {
                if (s_instancia == null) s_instancia = new Sistema();
                return s_instancia;
            }
        }

        public List<Publicacion> Publicacion
        {
            get { return _listaPublicaciones; }
        }

        #region ALTAS
        public void AltaArticulo(Articulo articulo)
        {
            if (articulo == null) throw new Exception("El artículo no puede ser nulo.");
            articulo.Validar();
            if (_listaArticulos.Contains(articulo)) throw new Exception("El artículo ingresado ya existe.");
            _listaArticulos.Add(articulo);
        }
      
        public void AltaUsuario(Usuario usuario)
        {
            if (usuario == null) throw new Exception("El usuario no puede ser nulo.");
            usuario.Validar();
            if (_listaUsuarios.Contains(usuario)) throw new Exception("El usuario ingresado ya existe.");
            _listaUsuarios.Add(usuario);
        }

        public void AltaPublicacion(Publicacion publicacion)
        {
            if (publicacion == null) throw new Exception("La publicación no puede ser nula.");
            publicacion.Validar();
            _listaPublicaciones.Add(publicacion);
        }
        #endregion

        #region AGREGACIONES
        public void AgregarArticuloAPublicacion(int idArticulo, int idPublicacion)
        {
            Publicacion publicacion = BuscarPublicacionPorId(idPublicacion);
            if (publicacion == null) throw new Exception("La publicación ingresada no existe.");

            Articulo articulo = BuscarArticuloPorId(idArticulo);
            publicacion.AgregarArticulo(articulo);
        }

        public void AgregarOfertaASubasta(int idCliente, int idSubasta, double monto, DateTime fecha)
        {
            Subasta subasta = BuscarSubastaPorId(idSubasta);
            if (subasta == null) throw new Exception("La subasta ingresada no existe.");

            Cliente cliente = BuscarClientePorId(idCliente);

            Oferta oferta = new Oferta(cliente, monto, fecha);
            subasta.AgregarOferta(oferta);
        }

        #endregion

        #region BUSQUEDAS
        public Articulo BuscarArticuloPorId(int id)
        {
            foreach (Articulo a in _listaArticulos)
            {
                if (a.Id == id) return a;
            }

            return null;
        }

        public Publicacion BuscarPublicacionPorId(int id)
        {
            foreach (Publicacion p in _listaPublicaciones)
            {
                if (p.Id == id) return p;
            }

            return null;
        }

        public Subasta BuscarSubastaPorId(int id)
        {
            foreach (Publicacion p in _listaPublicaciones)
            {
                if (p.Id == id)
                {
                    if (p is Subasta) return (Subasta) p;
                    return null;
                }
            }

            return null;
        }

        public Cliente BuscarClientePorId(int? id)
        {
            foreach (Usuario u in _listaUsuarios)
            {
                if (u.Id == id)
                {
                    if (u is Cliente) return (Cliente) u;
                    return null;
                }
            }

            return null;
        }
        #endregion

        #region LISTADOS
        //Listado de clientes filtrando la lista usuarios.
        public List<Cliente> ListarClientes()
        {
            List<Cliente> clientes = new List<Cliente>();

            foreach(Usuario u in _listaUsuarios)
            {
                if (u is Cliente)
                {
                    clientes.Add((Cliente) u);
                }
            }

            return clientes;
        }

        //Listado de las categorías disponibles recorriendo todos los artículos.
        public List<string> ListarCategorias()
        {
            List<string> categorias = new List<string>();

            foreach (Articulo a in _listaArticulos)
            {
                if (!categorias.Contains(a.Categoria))
                {
                    categorias.Add(a.Categoria);
                }
            }

            return categorias;
        }

        //Listado de artículos dada una categoría específica.
        public List<Articulo> ListarArticulosPorCategoria(string categoria)
        {
            List<Articulo> articulos = new List<Articulo>();

            foreach (Articulo a in _listaArticulos)
            {
                if (a.Categoria == categoria) articulos.Add(a);
            }

            return articulos;
        }

        //Listado de publicaciones que se encuentren entre dos fechas específicas.
        public List<Publicacion> ListarPublicacionesEntreDosFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin) throw new Exception("La fecha de inicio no puede ser mayor a la fecha final.");
            List<Publicacion> publicaciones = new List<Publicacion>();

            foreach (Publicacion p in _listaPublicaciones)
            {
                if (p.FechaPublicacion.Date >= fechaInicio.Date && p.FechaPublicacion.Date <= fechaFin.Date)
                {
                    publicaciones.Add(p);
                }
            }

            return publicaciones;
        }
        #endregion

        #region MÉTODOS AUXILIARES
        public string NormalizarString(string str)
        {
            // Pasa el string a mayúsculas y quita todas las tildes.
            str = str.ToUpper();

            str = str.Replace('Á', 'A');
            str = str.Replace('É', 'E');
            str = str.Replace('Í', 'I');
            str = str.Replace('Ó', 'O');
            str = str.Replace('Ú', 'U');

            str = str.Replace('À', 'A');
            str = str.Replace('È', 'E');
            str = str.Replace('Ì', 'I');
            str = str.Replace('Ò', 'O');
            str = str.Replace('Ù', 'U');

            return str;
        }

        //Devuelve una lista de articulos aleatorios (usado para precarga).
        private List<Articulo> ObtenerArticulosAleatorios(int cantidad)
        {
            Random random = new Random();
            List<Articulo> articulosSeleccionados = new List<Articulo>();

            while (articulosSeleccionados.Count < cantidad && articulosSeleccionados.Count < _listaArticulos.Count)
            {
                int indiceAleatorio = random.Next(_listaArticulos.Count);

                // Asegurarse de que no se selecciona el mismo artículo dos veces
                if (!articulosSeleccionados.Contains(_listaArticulos[indiceAleatorio]))
                {
                    articulosSeleccionados.Add(_listaArticulos[indiceAleatorio]);
                }
            }

            return articulosSeleccionados;
        }

        //Devuelve un cliente aleatorio (usado para precarga).
        private Cliente ObtenerClienteAleatorio()
        {
            List<Cliente> clientes = new List<Cliente>();
            clientes = ListarClientes();

            if (clientes.Count == 0) return null;

            Random random = new Random();
            int indiceAleatorio = random.Next(clientes.Count);

            return clientes[indiceAleatorio];
        }

        //Devuelve una lista de ofertas aleatoria (usado para precarga).
        private List<Oferta> ObtenerOfertasAleatorias()
        {
            List<Oferta> ofertas = new List<Oferta>();
            Random random = new Random();
            double montoAleatorio = random.Next(300);

            ofertas.Add(new Oferta(ObtenerClienteAleatorio(), montoAleatorio, DateTime.Today));

            return ofertas;
        }

        public void AgregarOferta(Subasta subasta, Oferta oferta)
        {
            if (subasta == null) throw new Exception("La subasta no puede ser nula.");
            if (oferta == null) throw new Exception("La oferta no puede ser nula.");

            if (subasta.Ofertas == null)
            {
                subasta.Ofertas = new List<Oferta>();
            }

            subasta.Ofertas.Add(oferta);
        }

        public Oferta ObtenerNuevaOferta(int idCliente, double monto)
        {
            Cliente cliente = BuscarClientePorId(idCliente);

            //falta validar monto

            if (cliente == null) throw new Exception($"Cliente con ID {idCliente} no encontrado.");

            Oferta nuevaOferta = new Oferta(cliente, monto, DateTime.Today);
          
            return nuevaOferta;
        }

        public Usuario Login(string email, string contrasenia)
        {
            Usuario usuarioBuscado = null;
            int i = 0;
            while (usuarioBuscado == null && i < _listaUsuarios.Count)
            {
                if (_listaUsuarios[i].Email == email && _listaUsuarios[i].Clave == contrasenia) usuarioBuscado = _listaUsuarios[i];
                i++;
            }
            return usuarioBuscado;
        }

        public Publicacion ObtenerPublicacionPorId(int id)
        {
            Publicacion buscado = null;
            int i = 0;
            while (i < _listaPublicaciones.Count && buscado == null)
            {
                if (_listaPublicaciones[i].Id == id) buscado = _listaPublicaciones[i];
                i++;
            }

            return buscado;
        }

        public void ProcesarCompra(int idCliente , int idVenta)
        {
            // validaciones
            Publicacion venta = BuscarPublicacionPorId(idVenta);
            if (venta == null) throw new Exception("La publicacion que quiere comprar no existe.");
            if (venta.EstadoPublicacion != EstadoPublicacion.ABIERTA) throw new Exception("La publicacion que quiere comprar ya no está disponible.");

            Cliente cliente = BuscarClientePorId(idCliente);
            if (cliente == null) throw new Exception("Cliente inválido.");
            if (cliente.Saldo < venta.CalcularPrecio()) throw new Exception("Saldo insuficiente.");

            //cerrar la publicacion y descontar saldo al cliuente
            venta.EstadoPublicacion = EstadoPublicacion.CERRADA;
            cliente.Saldo -= venta.CalcularPrecio();
        }

        public void cargarSaldo(double saldo, int? id) 
        {
            if (saldo <= 0) throw new Exception("Debe ingresar un valor positivo para poder incrementar el saldo");
            Cliente c = BuscarClientePorId(id);
            if (c == null) throw new Exception("El usuario es inválido");
            c.Saldo += saldo;
        }
     
        #endregion

        #region PRECARGAS
        private void PrecargarUsuarios()
        {
            // Precarga de clientes
            AltaUsuario(new Cliente("Pedro", "Perez", "PedroPerez@gmail.com", "pedroPe123", 1900));
            AltaUsuario(new Cliente("Laura", "Gomez", "LauraGomez@gmail.com", "lauraG456", 2500));
            AltaUsuario(new Cliente("Carlos", "Diaz", "CarlosDiaz@gmail.com", "carlosD789", 3200));
            AltaUsuario(new Cliente("Ana", "Martinez", "AnaMartinez@gmail.com", "anaM1012", 1500));
            AltaUsuario(new Cliente("Juan", "Lopez", "JuanLopez@gmail.com", "juanL202", 2800));
            AltaUsuario(new Cliente("Maria", "Hernandez", "MariaHernandez@gmail.com", "mariaH303", 4100));
            AltaUsuario(new Cliente("Luis", "Garcia", "LuisGarcia@gmail.com", "luisG404", 1800));
            AltaUsuario(new Cliente("Sofia", "Rodriguez", "SofiaRodriguez@gmail.com", "sofiaR505", 3300));
            AltaUsuario(new Cliente("Diego", "Sanchez", "DiegoSanchez@gmail.com", "diegoS606", 1200));
            AltaUsuario(new Cliente("Lucia", "Ramirez", "LuciaRamirez@gmail.com", "luciaR707", 2950));

            // Precarga de administradores
            AltaUsuario(new Administrador("Santiago", "Fernandez", "SantiMas@gmail.com", "Firulais33"));
            AltaUsuario(new Administrador("Valeria", "Torres", "ValeriaTorres@gmail.com", "valTorres44"));
        }

        private void PrecargarArticulos()
        {
            AltaArticulo(new Articulo("Laptop", "Electrónica", 2000));
            AltaArticulo(new Articulo("Smartphone", "Electrónica", 1000));
            AltaArticulo(new Articulo("Televisor", "Electrónica", 1500));
            AltaArticulo(new Articulo("Cafetera", "Hogar", 300));
            AltaArticulo(new Articulo("Lavadora", "Electrodomésticos", 2500));
            AltaArticulo(new Articulo("Secadora", "Electrodomésticos", 2300));
            AltaArticulo(new Articulo("Refrigerador", "Electrodomésticos", 3000));
            AltaArticulo(new Articulo("Silla de oficina", "Muebles", 150));
            AltaArticulo(new Articulo("Escritorio", "Muebles", 400));
            AltaArticulo(new Articulo("Cama", "Muebles", 700));
            AltaArticulo(new Articulo("Monitor", "Electrónica", 500));
            AltaArticulo(new Articulo("Teclado mecánico", "Electrónica", 100));
            AltaArticulo(new Articulo("Ratón inalámbrico", "Electrónica", 50));
            AltaArticulo(new Articulo("Audífonos", "Electrónica", 200));
            AltaArticulo(new Articulo("Bicicleta", "Deportes", 800));
            AltaArticulo(new Articulo("Pelota de fútbol", "Deportes", 40));
            AltaArticulo(new Articulo("Zapatillas deportivas", "Deportes", 120));
            AltaArticulo(new Articulo("Camisa", "Ropa", 30));
            AltaArticulo(new Articulo("Pantalón", "Ropa", 50));
            AltaArticulo(new Articulo("Zapatos", "Ropa", 80));
            AltaArticulo(new Articulo("Guitarra", "Música", 600));
            AltaArticulo(new Articulo("Piano eléctrico", "Música", 1500));
            AltaArticulo(new Articulo("Batería", "Música", 2000));
            AltaArticulo(new Articulo("Libro de cocina", "Libros", 25));
            AltaArticulo(new Articulo("Novela", "Libros", 20));
            AltaArticulo(new Articulo("Enciclopedia", "Libros", 120));
            AltaArticulo(new Articulo("Plancha", "Hogar", 60));
            AltaArticulo(new Articulo("Aspiradora", "Hogar", 200));
            AltaArticulo(new Articulo("Ventilador", "Electrodomésticos", 150));
            AltaArticulo(new Articulo("Cámara fotográfica", "Electrónica", 700));
            AltaArticulo(new Articulo("Dron", "Electrónica", 1200));
            AltaArticulo(new Articulo("Smartwatch", "Electrónica", 300));
            AltaArticulo(new Articulo("Tablet", "Electrónica", 600));
            AltaArticulo(new Articulo("Altavoces", "Electrónica", 250));
            AltaArticulo(new Articulo("Impresora", "Oficina", 400));
            AltaArticulo(new Articulo("Proyector", "Oficina", 800));
            AltaArticulo(new Articulo("Calculadora", "Oficina", 20));
            AltaArticulo(new Articulo("Cámara de seguridad", "Electrónica", 500));
            AltaArticulo(new Articulo("Router Wi-Fi", "Electrónica", 120));
            AltaArticulo(new Articulo("Modem", "Electrónica", 100));
            AltaArticulo(new Articulo("Microondas", "Electrodomésticos", 300));
            AltaArticulo(new Articulo("Horno eléctrico", "Electrodomésticos", 400));
            AltaArticulo(new Articulo("Licuadora", "Hogar", 150));
            AltaArticulo(new Articulo("Tostadora", "Hogar", 80));
            AltaArticulo(new Articulo("Set de cuchillos", "Cocina", 50));
            AltaArticulo(new Articulo("Olla a presión", "Cocina", 200));
            AltaArticulo(new Articulo("Sartén antiadherente", "Cocina", 100));
            AltaArticulo(new Articulo("Mesa de comedor", "Muebles", 1000));
            AltaArticulo(new Articulo("Set de cucharas", "Cocina", 100));
        }

        private void PrecargarPublicaciones()
        {
            // PROFE YA LO HABLAMOS: Constructor solo para precargas (hay otro para las altas que no incluye las listas)
            AltaPublicacion(new Subasta("Subasta 1", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 15), ObtenerArticulosAleatorios(2), null));
            AltaPublicacion(new Subasta("Subasta 2", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 18), ObtenerArticulosAleatorios(6), ObtenerOfertasAleatorias()));
            AltaPublicacion(new Subasta("Subasta 3", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 20), ObtenerArticulosAleatorios(4), null));
            AltaPublicacion(new Subasta("Subasta 4", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 22), ObtenerArticulosAleatorios(3), ObtenerOfertasAleatorias()));
            AltaPublicacion(new Subasta("Subasta 5", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 25), ObtenerArticulosAleatorios(4), null));
            AltaPublicacion(new Subasta("Subasta 6", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 27), ObtenerArticulosAleatorios(2), ObtenerOfertasAleatorias()));
            AltaPublicacion(new Subasta("Subasta 7", EstadoPublicacion.ABIERTA, new DateTime(2024, 10, 01), ObtenerArticulosAleatorios(1), null));
            AltaPublicacion(new Subasta("Subasta 8", EstadoPublicacion.ABIERTA, new DateTime(2024, 10, 03), ObtenerArticulosAleatorios(4), null));
            AltaPublicacion(new Subasta("Subasta 9", EstadoPublicacion.ABIERTA, new DateTime(2024, 10, 05), ObtenerArticulosAleatorios(4), null));
            AltaPublicacion(new Subasta("Subasta 10", EstadoPublicacion.ABIERTA, new DateTime(2024, 10, 06), ObtenerArticulosAleatorios(6), ObtenerOfertasAleatorias()));

            AltaPublicacion(new Venta("Venta 1", EstadoPublicacion.ABIERTA, new DateTime(2024, 10, 01), ObtenerArticulosAleatorios(3), true));
            AltaPublicacion(new Venta("Venta 2", EstadoPublicacion.ABIERTA, new DateTime(2024, 02, 05), ObtenerArticulosAleatorios(4), false));
            AltaPublicacion(new Venta("Venta 3", EstadoPublicacion.ABIERTA, new DateTime(2024, 02, 08), ObtenerArticulosAleatorios(1), false));
            AltaPublicacion(new Venta("Venta 4", EstadoPublicacion.ABIERTA, new DateTime(2024, 05, 12), ObtenerArticulosAleatorios(5), false));
            AltaPublicacion(new Venta("Venta 5", EstadoPublicacion.ABIERTA, new DateTime(2024, 09, 15), ObtenerArticulosAleatorios(7), true));
            AltaPublicacion(new Venta("Venta 6", EstadoPublicacion.ABIERTA, new DateTime(2024, 08, 20), ObtenerArticulosAleatorios(2), false));
            AltaPublicacion(new Venta("Venta 7", EstadoPublicacion.ABIERTA, new DateTime(2024, 07, 25), ObtenerArticulosAleatorios(7), true));
            AltaPublicacion(new Venta("Venta 8", EstadoPublicacion.ABIERTA, new DateTime(2024, 04, 30), ObtenerArticulosAleatorios(3), false));
            AltaPublicacion(new Venta("Venta 9", EstadoPublicacion.ABIERTA, new DateTime(2024, 03, 02), ObtenerArticulosAleatorios(7), false));
            AltaPublicacion(new Venta("Venta 10", EstadoPublicacion.ABIERTA, new DateTime(2024, 04, 05), ObtenerArticulosAleatorios(1), true));
        }

        #endregion
    }
}