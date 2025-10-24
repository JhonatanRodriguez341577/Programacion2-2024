using Dominio.Enums;
using Dominio.Interfaces;

namespace Dominio
{
    public abstract class Publicacion : IValidable
    {
        protected int _id;
        private static int s_ultId = 1;
        protected string _nombre;
        protected EstadoPublicacion _estado;
        protected DateTime _fechaPublicacion;
        protected List<Articulo> _articulos = new List<Articulo>();
        protected Cliente _clienteComprador;
        protected Usuario _usuarioFinalizador;
        protected DateTime _fechaFinalizada;

        protected Publicacion(string nombre, EstadoPublicacion estado, DateTime fechaPublicacion)
        {
            _id = s_ultId++;
            _nombre = nombre;
            _estado = estado;
            _fechaPublicacion = fechaPublicacion;
        }

        // Constructor sólo para precargas
        protected Publicacion(string nombre, EstadoPublicacion estado, DateTime fechaPublicacion, List<Articulo> articulos)
        {
            _id = s_ultId++;
            _nombre = nombre;
            _estado = estado;
            _fechaPublicacion = fechaPublicacion;
            _articulos = articulos;
        }

        public int Id { get { return _id; } }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public EstadoPublicacion EstadoPublicacion
        {
            get { return _estado; }
            set { _estado = value; }
        }

        public List<Articulo> Articulos { get { return _articulos; } }

        public DateTime FechaPublicacion { get { return _fechaPublicacion; } }


        public virtual void AgregarArticulo(Articulo articulo)
        {
            if (articulo == null) throw new Exception("El artículo no puede ser nulo.");
            articulo.Validar();
            if (_articulos.Contains(articulo)) throw new Exception("El artículo ingresado ya se encuentra en la publicación.");
            _articulos.Add(articulo);
        }

        public virtual void Validar()
        {
            if (string.IsNullOrEmpty(_nombre)) throw new Exception("El nombre no puede estar vacío.");
            if (_fechaPublicacion < new DateTime(2024, 1, 1) || _fechaPublicacion > DateTime.Today) throw new Exception("La fecha de publicación es inválida.");
        }

        public override string ToString()
        {
            return $"Nº{_id}: {_nombre} | Estado: {_estado} | Fecha publicación: {_fechaPublicacion.ToShortDateString()}";
        }

        public virtual double CalcularPrecio()
        {
            double total = 0;

            foreach (Articulo a in _articulos)
            {
                total += a.Precio;
            }

            return total;
        }

        public virtual bool esOfertaRelampago()
        {
            return false;
        }

        public virtual bool esVenta()
        {
            return false;
        }
        public virtual bool esSubasta()
        {
            return false;
        }

       
}
}