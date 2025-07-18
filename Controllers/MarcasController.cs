using P0006.Metodos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P0006.Models.ViewModels;
using P0006.Models;

namespace P0006.Controllers
{
    public class MarcasController : Controller
    {
        // GET: Marcas
        public ActionResult Marcas()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Acceder");
            return View();
        }

        [HttpGet]
        public JsonResult ConsultaMarcas()
        {
            List<Marcas> oMarcas = new List<Marcas>();
            oMarcas = MarcasMetodos.Instancia.Listar();
            return Json(new { data = oMarcas }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarMarcas(Marcas marca)
        {
            try
            {
                bool respuesta = false;
                respuesta = (marca.IdMarca == 0) ? MarcasMetodos.Instancia.Registrar(marca) : MarcasMetodos.Instancia.Modificar(marca);
                return Json(new { resultado = respuesta });
            }
            catch (Exception ex)
            {
                // Log the error and return it for debugging
                return Json(new { resultado = false, error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult BorrarMarcas(int Id)
        {
            bool respuesta = false;
            respuesta = MarcasMetodos.Instancia.Eliminar(Id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.DenyGet);
        }
    }
}