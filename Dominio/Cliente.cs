namespace Dominio
{
    public class Cliente : Usuario
    {
        public double Saldo { get; set; }

        public Cliente(string nombre, string apellido, string email, string clave, double saldo) : base(nombre, apellido, email, clave)
        {
            Saldo = saldo;
        }

        public override void Validar()
        {
            base.Validar();
            ValidarSaldo();
        }

        public override string ToString()
        {
            return $"{_id}: {Nombre} {Apellido} | {Email} | ${Saldo}";
        }
        
        private void ValidarSaldo()
        {
            if (Saldo < 0) throw new Exception("El saldo debe ser un monto positivo.");
            
        }

        public override string Rol()
        {
            return "Cliente";
        }
    }
}
