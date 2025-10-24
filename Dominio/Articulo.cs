using Dominio.Interfaces;

namespace Dominio
{
    public class Articulo : IValidable
    {
        private int _id;
        private static int s_ultId = 1;
        private string _nombre;
        private string _categoria;
        private double _precio;

        public Articulo(string nombre, string categoria, double precio)
        {
            _id = s_ultId++;
            _nombre = nombre;
            _categoria = categoria;
            _precio = precio;
        }

        public int Id { get { return _id; } }
        public string Nombre { get { return _nombre; } }

        public string Categoria { get { return _categoria; } }
        public double Precio { get { return _precio; } }

        public void Validar()
        {
            if (string.IsNullOrEmpty(_nombre)) throw new Exception("El nombre no puede estar vacío.");
            if (string.IsNullOrEmpty(_categoria)) throw new Exception("La categoría no puede estar vacía.");
            if (_precio <= 0) throw new Exception("El precio debe ser mayor a $0.");
        }

        public override string ToString()
        {
            return $"Artículo Nº{_id}: {_nombre} - ${_precio}";
        }

        public override bool Equals(object? obj)
        {
            Articulo a = obj as Articulo;
            return a != null && this._nombre == a._nombre && this._categoria == a._categoria && this._precio == a._precio;
        }
    }
}
