using Microsoft.AspNetCore.Mvc;
using Dominio;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;



namespace web.Controllers
{
    public class UsuariosController : Controller
    {
        private Sistema miSistema = Sistema.Instancia;

        public IActionResult Registrarse()
        {
            if (HttpContext.Session.GetString("rol") != null)
            {
                return View("NoAutorizado");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Registrarse(string nombre , string apellido, string email, string contrasenia)
        {
            try 
            {
                if (string.IsNullOrEmpty(nombre)) throw new Exception("nombre no puede ser vacío");
                if (string.IsNullOrEmpty(apellido)) throw new Exception("apellido no puede ser vacío");
                if (string.IsNullOrEmpty(email)) throw new Exception("email no puede ser vacío");
                if (string.IsNullOrEmpty(contrasenia)) throw new Exception("contraseña no puede ser vacía");

                Usuario usuario = new Cliente(nombre, apellido, email, contrasenia, 2000);
                miSistema.AltaUsuario(usuario);
                TempData["Exito"] = "¡Registro exitoso!";
                return RedirectToAction("login", "usuarios");
            }
            catch (Exception ex)
            {
                ViewBag.Nombre = nombre;
                ViewBag.Apellido = apellido;
                ViewBag.Email = email;
                ViewBag.Contrasenia = contrasenia;
                ViewBag.Error = ex.Message;
               
            }
           return View("Registrarse");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("rol") != null)
            {        
                return View("NoAutorizado");
            }

            ViewBag.Exito = TempData["Exito"];
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email , string contrasenia)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) throw new Exception("Debe ingresar un email");
                if (string.IsNullOrEmpty(contrasenia)) throw new Exception("Debe ingresar una contraseña");
                Usuario usuario = miSistema.Login(email, contrasenia);
                if (usuario == null) throw new Exception("Email o contraseña incorrectas");

                HttpContext.Session.SetInt32("Id", usuario.Id);
                HttpContext.Session.SetString("email", email);
                HttpContext.Session.SetString("rol", usuario.Rol());

                return RedirectToAction("publicacion","publicaciones");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message ;
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Perfil()
        {
            int? id = HttpContext.Session.GetInt32("Id");

            if (id != null)
            {
                Cliente p = miSistema.BuscarClientePorId(id);
                ViewBag.cliente = p;
                return View();
            }
            else
            {
                ViewBag.Error = "No se encontró un ID en la sesión.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult Perfil(double saldo)
        {
             
            try
            {
                int? id = HttpContext.Session.GetInt32("Id");
                
                if (id != null)
                {
                    ViewBag.cliente = miSistema.BuscarClientePorId(id);
                    miSistema.cargarSaldo(saldo, id);
                    ViewBag.Exito = "Saldo cargado correctamente";
                }
                else
                {                  
                    ViewBag.Error = "No se encontró un usuario en la sesión.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }
    }
}
