using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace web.Controllers
{
    public class PublicacionesController : Controller
    {
        private Sistema miSistema = Sistema.Instancia;
        public IActionResult Publicacion()
        { 
            if (HttpContext.Session.GetString("rol") == null)
            {
                return View("NoAutorizado");
            }

            ViewBag.Listado = miSistema.Publicacion;
            return View();
        }

        [HttpGet]
        public IActionResult ComprarVenta(int id)
        {
            if (HttpContext.Session.GetString("rol") == null)
            {
                return View("NoAutorizado");
            }
            Publicacion buscado = miSistema.ObtenerPublicacionPorId(id);
            ViewBag.Publicacion = buscado;
            return View();
        }

        [HttpPost]
        public IActionResult ComprarVenta(int idVenta, int idCliente)
        {
            if (HttpContext.Session.GetString("rol") == null)
            {
                return View("NoAutorizado");
            }

            try
            {
                Publicacion buscado = miSistema.ObtenerPublicacionPorId(idVenta);
                ViewBag.Publicacion = buscado;
                miSistema.ProcesarCompra(idCliente, idVenta);
                ViewBag.Exito = "Su compra se ha procesado exitosamente.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult ComprarSubasta(int id)
        {
            if (HttpContext.Session.GetString("rol") == null)
            {
                return View("NoAutorizado");
            }
            Publicacion buscado = miSistema.ObtenerPublicacionPorId(id);
            ViewBag.Publicacion = buscado;
            return View();
        }

        [HttpPost]
        public IActionResult ComprarSubasta(int idSubasta, int idCliente, double monto)
        {
            if (HttpContext.Session.GetString("rol") == null)
            {
                return View("NoAutorizado");
            }

            try
            {
                Publicacion buscado = miSistema.ObtenerPublicacionPorId(idSubasta);
                ViewBag.Publicacion = buscado;

                Subasta subasta = miSistema.BuscarSubastaPorId(idSubasta);
                Oferta nuevaOferta = miSistema.ObtenerNuevaOferta(idCliente, monto);

                //falta validar si el nuevo monto es mayor al anterior

                miSistema.AgregarOferta(subasta, nuevaOferta);
                ViewBag.Exito = "Su oferta se ha procesado exitosamente.";

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
