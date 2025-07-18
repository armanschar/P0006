using P0006.Metodos;
using P0006.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P0006.Controllers
{
    public class TiposController : Controller
    {
        // GET: Tipos
        public ActionResult Tipos()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Acceder");
            return View();
        }

        [HttpGet]
        public JsonResult ConsultaTipos()
        {
            List<Tipos> oLista = new List<Tipos>();
            oLista = TiposMetodos.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarTipo(Tipos oCat)
        {
            bool respuesta = false;
            respuesta = (oCat.IdTipo == 0) ? TiposMetodos.Instancia.Registrar(oCat) : TiposMetodos.Instancia.Modificar(oCat);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BorrarTipos (int Id)
        {
            bool respuesta = false;
            respuesta = TiposMetodos.Instancia.Eliminar(Id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

    }
}