using Dominio;

namespace Consola
{
    internal class Program
    {
        private static Sistema sistema;

        static void Main(string[] args)
        {
            sistema = new Sistema();

            string opc = "";

            while (opc != "0")
            {
                MostrarMenu();
                opc = PedirString("");

                switch (opc)
                {
                    case "1":
                        AltaArticulo();
                        break;
                    case "2":
                        ListarClientes();
                        break;
                    case "3":
                        ListarArticulosPorCategoria();
                        break;
                    case "4":
                        ListarPublicacionesEntreDosFechas();
                        break;
                    case "0":
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        MostrarError("Opción inválida.");
                        break;
                }
            }
        }

        #region MÉTODOS AUXILIARES
        static void MostrarMenu()
        {
            Console.Clear();
            MostrarMensajeColor(ConsoleColor.Cyan, "****************");
            MostrarMensajeColor(ConsoleColor.Cyan, "      MENÚ      ");
            MostrarMensajeColor(ConsoleColor.Cyan, "****************");
            Console.WriteLine();
            Console.WriteLine("1 - Alta de artículo");
            Console.WriteLine("2 - Listar clientes");
            Console.WriteLine("3 - Listar artículos por categoría");
            Console.WriteLine("4 - Listar publicaciones por fecha");
            Console.WriteLine("0 - Salir");
        }

        static string PedirString(string mensaje)
        {
            Console.WriteLine(mensaje);
            string str = Console.ReadLine();
            Console.WriteLine();
            return str;
        }

        static int PedirInt(string mensaje) {
            int numero;

            while (true)
            {
                Console.Write(mensaje);

                if (int.TryParse(Console.ReadLine(), out numero))
                {
                    Console.WriteLine();
                    return numero;
                }

                MostrarError("ERROR: El número ingresado no es válido.");
            }
        }

        static double PedirDouble(string mensaje)
        {
            double numero;

            while (true)
            {
                Console.WriteLine(mensaje);

                if (double.TryParse(Console.ReadLine(), out numero))
                {
                    Console.WriteLine();
                    return numero;
                }

                MostrarError("ERROR: El número ingresado no es válido.");
            }
        }

        static bool PedirBool(string mensaje)
        {
            while (true)
            {
                Console.WriteLine($"{mensaje} [S/N]:");
                string datoString = Console.ReadLine().ToUpper();

                if (datoString == "S")
                {
                    Console.WriteLine();
                    return true;
                }
                else if (datoString == "N")
                {
                    Console.WriteLine();
                    return false;
                }
                else
                {
                    MostrarError("ERROR: Opción inválida.");
                }
            }
        }

        static DateTime PedirFecha(string mensaje)
        {
            while (true)
            {
                Console.Write($"{mensaje} [dd/MM/yyyy]:\n");

                if (DateTime.TryParse(Console.ReadLine(), out DateTime fecha))
                {
                    Console.WriteLine();
                    return fecha;
                }

                MostrarError("ERROR: La fecha no respeta el formato dd/MM/yyyy.");
            }
        }

        static void MostrarMensajeColor(ConsoleColor color, string mensaje)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(mensaje + "\n");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensaje);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void PressToContinue()
        {
            Console.WriteLine("Presione cualquier tecla para volver al menú...");
            Console.ReadKey();
        }

        #endregion
        
        private static void AltaArticulo()
        {
            Console.Clear();
            MostrarMensajeColor(ConsoleColor.Yellow, "ALTA DE ARTÍCULO");
            Console.WriteLine();

            string nombre = PedirString("Ingrese el nombre del artículo: ");
            string categoria = PedirString("Ingrese la categoría del artículo: ");
            double precio = PedirDouble("Ingrese el precio del artículo: ");

            try
            {
                Articulo articulo = new Articulo(nombre, categoria, precio);
                sistema.AltaArticulo(articulo);

                Console.Clear();
                MostrarMensajeColor(ConsoleColor.Yellow, "ALTA DE ARTÍCULO");
                Console.WriteLine();
                MostrarExito("Artículo insertado satisfactoriamente.");
            }
            catch (Exception ex)
            {
                Console.Clear();
                MostrarError(ex.Message);
            }
            finally
            {
                PressToContinue();
            }
        }

        private static void ListarClientes()
        {
            Console.Clear();
            MostrarMensajeColor(ConsoleColor.Yellow, "LISTADO DE CLIENTES");
            Console.WriteLine();

            try
            {
                List<Cliente> clientes = new List<Cliente>();
                clientes = sistema.ListarClientes();

                if (clientes.Count == 0) throw new Exception("No se encontró ningún cliente.");

                foreach (Cliente c in clientes)
                {
                    Console.WriteLine(c);
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.Clear();
                MostrarError(ex.Message);
            }
            finally
            {
                PressToContinue();
            }
        }

        private static void ListarArticulosPorCategoria()
        {
            Console.Clear();
            MostrarMensajeColor(ConsoleColor.Yellow, "LISTADO DE ARTÍCULOS POR CATEGORÍA");
            Console.WriteLine("\nCategorías disponibles:");

            try
            {
                List<string> categorias = sistema.ListarCategorias();
                int contador = 1;

                if (categorias.Count == 0) throw new Exception("No se encontró ninguna categoría.");

                foreach (string c in categorias)
                {
                    Console.WriteLine($"{contador}- {c}");
                    contador++;
                }

                int categoriaSeleccionada = PedirInt("\nIngrese el número de categoría a listar: ");

                List<Articulo> articulos = new List<Articulo>();
                articulos = sistema.ListarArticulosPorCategoria(categorias[categoriaSeleccionada - 1]);

                Console.Clear();
                MostrarMensajeColor(ConsoleColor.Yellow, $"LISTADO DE ARTÍCULOS POR CATEGORÍA: {sistema.NormalizarString(categorias[categoriaSeleccionada - 1])}");
                Console.WriteLine();

                foreach (Articulo a in articulos)
                {
                    Console.WriteLine(a);
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.Clear();
                MostrarError(ex.Message);
            }
            finally
            {
                PressToContinue();
            }
        }

        private static void ListarPublicacionesEntreDosFechas()
        {
            Console.Clear();
            MostrarMensajeColor(ConsoleColor.Yellow, "LISTADO DE PUBLICACIONES POR FECHA\n");

            try
            {
                DateTime fechaInicio = PedirFecha("Ingrese la fecha de inicio");
                DateTime fechaFinal = PedirFecha("Ingrese la fecha de fin");

                Console.Clear();
                MostrarMensajeColor(ConsoleColor.Yellow, "LISTADO DE PUBLICACIONES POR FECHA\n");

                List<Publicacion> publicaciones = new List<Publicacion>();

                publicaciones = sistema.ListarPublicacionesEntreDosFechas(fechaInicio, fechaFinal);

                if (publicaciones.Count == 0) throw new Exception("No se encontró ninguna publicación entre las dos fechas seleccionadas.");

                foreach (Publicacion p in publicaciones)
                {
                    Console.WriteLine(p);
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.Clear();
                MostrarError(ex.Message);
            }
            finally
            {
                PressToContinue();
            }
        }
    }
}