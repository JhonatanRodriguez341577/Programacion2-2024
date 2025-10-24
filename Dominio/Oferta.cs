using Dominio.Interfaces;

namespace Dominio
{
    public class Oferta : IValidable
    {
        private int _id;
        private static int s_ultId = 1;
        private Cliente _cliente;
        private double _monto;
        private DateTime _fechaRealizada;

        public Oferta(Cliente cliente, double monto, DateTime fechaRealizada)
        {
            _id = s_ultId++;
            _cliente = cliente;
            _monto = monto;
            _fechaRealizada = fechaRealizada;
        }

        public double Monto { get { return _monto; } }

        public Cliente Cliente { get { return _cliente; } }

        public void Validar()
        {
            if (_cliente == null) throw new Exception("El cliente no puede ser nulo.");
            _cliente.Validar();
            if (_monto <= 0) throw new Exception("El monto debe ser mayor a $0.");
            if (_fechaRealizada < new DateTime(2024, 1, 1) || _fechaRealizada > DateTime.Today) throw new Exception("La fecha realizada es inválida.");
        }
    }
}