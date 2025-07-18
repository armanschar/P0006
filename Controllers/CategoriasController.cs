using P0006.Metodos;
using P0006.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P0006.Controllers
{
    public class CategoriasController : Controller
    {
        // GET: Categorias
        public ActionResult Categorias()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Login", "Acceder");
            return View();
        }

        [HttpGet]
        public JsonResult ConsultaCategorias()
        {
            List<Categorias> oCategorias = new List<Categorias>();
            oCategorias = CategoriasMetodos.Instancia.Listar();
            return Json(new { data = oCategorias }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarCategorias(Categorias oCategoria)
        {
            bool respuesta = false;
            respuesta = (oCategoria.IdCategoria == 0) ? CategoriasMetodos.Instancia.Registrar(oCategoria) : CategoriasMetodos.Instancia.Modificar(oCategoria);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BorrarCategorias(int Id)
        {
            bool respuesta = false;
            respuesta = CategoriasMetodos.Instancia.Eliminar(Id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
}