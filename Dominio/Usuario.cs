using Dominio.Interfaces;

namespace Dominio
{
    public abstract class Usuario : IValidable
    {
        public int _id;
        public static int s_ultId = 1;
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }


        public Usuario(string nombre, string apellido, string email, string clave)
        {
            _id = s_ultId++;
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Clave = clave;
        }

        public virtual void Validar()
        {
            if (string.IsNullOrEmpty(Nombre)) throw new Exception("El nombre no puede estar vacío.");
            if (string.IsNullOrEmpty(Apellido)) throw new Exception("El apellido no puede estar vacío.");
            if (!EmailValido(Email)) throw new Exception("El email ingresado es inválido.");
            if (Clave.Length < 8) throw new Exception("La contraseña debe contener un mínimo de 8 caracteres.");
        }

        public int Id { get { return _id; } }

        private bool EmailValido(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            if (!email.Contains("@")) return false;
            if (email.Contains(" ")) return false;
            if (email.StartsWith("@")) return false;
            if (email.EndsWith("@")) return false;

            return true;
        }

        public override bool Equals(object? obj)
        {
            Usuario u = obj as Usuario;
            return u != null && this.Email == u.Email;
        }

        public virtual string Rol() 
        {
            return "";
        }
    }
}