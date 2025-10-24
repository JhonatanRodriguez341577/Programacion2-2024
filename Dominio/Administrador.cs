namespace Dominio
{
    public class Administrador : Usuario
    {
        public Administrador(string nombre, string apellido, string email, string clave) : base(nombre, apellido, email, clave)
        {
        }

        public override string Rol()
        {
            return "Admin";
        }
    }
}