using Dominio.Enums;

namespace Dominio
{
    public class Venta : Publicacion
    {
        private bool _ofertaRelampago;

        public Venta(string nombre, EstadoPublicacion estado, DateTime fechaPublicacion, bool ofertaRelampago) : base(nombre, estado, fechaPublicacion)
        {
            _ofertaRelampago = ofertaRelampago;
        }

        // Constructor sólo para precargas
        public Venta(string nombre, EstadoPublicacion estado, DateTime fechaPublicacion, List<Articulo> articulos, bool ofertaRelampago) : base(nombre, estado, fechaPublicacion, articulos)
        {
            _ofertaRelampago = ofertaRelampago;
        }

        public bool OfertaRelampago { get { return _ofertaRelampago; } }

        public override double CalcularPrecio()
        {
            if (OfertaRelampago) return base.CalcularPrecio() * 0.8;
            return base.CalcularPrecio();
        }

        public override bool esOfertaRelampago()
        {
            if (OfertaRelampago)
            {
                return true;
            }
            return base.esOfertaRelampago();
        }

        public override bool esVenta()
        {
            return true;
        }

    }
}
