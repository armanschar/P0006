using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P0006.Models;
using P0006.Models.ViewModels;

namespace P0006.Controllers
{
    public class TipdocController : Controller
    {

        // GET: Tipdoc
        public ActionResult TipdocQuery()
        {
            List<TipdocQueryViewModels> lst = null;

            using (DBMVCEntities db = new DBMVCEntities())
            {
                //realizamos la lectura a la tabla con linq
                lst = (from d in db.TIPDOCs
                       where d.ESTATUS == 1
                       orderby d.DESCRIPCION

                       //llenamos el modelo que se llama TipdocQueryViewModels
                       select new TipdocQueryViewModels
                       {
                           Id = d.ID, //asigna los valores a cada columna del mdoelo
                           Tipodoc = d.TIPDOC1,
                           Descripcion = d.DESCRIPCION,
                           Origen = d.ORIGEN
                       }).ToList();
            }
            return View(lst); //enviamos la data hacia la vista llamada Query
        }

        [HttpPost]
        public ActionResult TipdocDelete(int Id)
        {
            using (var db = new DBMVCEntities())
            {
                var oTipo = db.TIPDOCs.Find(Id);
                if (oTipo == null)
                    return Content("0"); // Si no se encuentra el registro, retornamos 0

                oTipo.ESTATUS = 3;
                db.Entry(oTipo).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            return Content("1"); // Si se elimina correctamente, retornamos 1
        }

        [HttpGet]
        public ActionResult TipdocAdd()
        {
            return View(); //muestra la vista vacia
        }

        [HttpPost]
        public ActionResult TipdocAdd(TipdocAddViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new DBMVCEntities())
            {
                TIPDOC oTipo = new TIPDOC();

                // Obtener la primera letra del tipo de documento, y pasarla a mayúscula para el contador
                string prefix = !string.IsNullOrEmpty(model.Tipodoc) ? model.Tipodoc.Substring(0, 1).ToUpper() : "X";

                // Encontrar el último contador del tipo de documento y calcular el siguiente número
                var lastContador = db.TIPDOCs
                    .Where(t => t.CONTADOR.StartsWith(prefix))
                    .OrderByDescending(t => t.CONTADOR)
                    .Select(t => t.CONTADOR)
                    .FirstOrDefault();

                // Si no hay un contador previo, comenzamos con 1
                int nextNumber = 1;
                if (!string.IsNullOrEmpty(lastContador) && lastContador.Length > 1)
                {
                    string numberPart = lastContador.Substring(1);
                    if (int.TryParse(numberPart, out int lastNumber))
                    {
                        nextNumber = lastNumber + 1;
                    }
                }

                //Asignamos el contador con el prefijo obtenido y el número en formato de 3 dígitos
                oTipo.CONTADOR = $"{prefix}{nextNumber.ToString("D3")}"; 

                oTipo.TIPDOC1 = model.Tipodoc;
                oTipo.DESCRIPCION = model.Descripcion;
                oTipo.ORIGEN = model.Origen;
                oTipo.CTADEBITO = model.CtaDebito;
                oTipo.CTACREDITO = model.CtaCredito;
                oTipo.ESTATUS = 1;

                db.TIPDOCs.Add(oTipo);
                db.SaveChanges();
            }

            return RedirectToAction(Url.Content("~/TipdocQuery")); //muestra la consulta con el nuevo registro
        }

        [HttpGet]
        public ActionResult TipdocEdit(int Id)
        {
            TipdocEditViewModels model = new TipdocEditViewModels();
            using (var db = new DBMVCEntities())
            {
                var oTipo = db.TIPDOCs.Find(Id);

                model.Id = oTipo.ID;
                model.Tipodoc = oTipo.TIPDOC1;
                model.Descripcion = oTipo.DESCRIPCION;
                model.Origen = oTipo.ORIGEN;
                model.CtaDebito = oTipo.CTADEBITO;
                model.CtaCredito = oTipo.CTACREDITO;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult TipdocEdit(TipdocEditViewModels model)
        {
            if (!ModelState.IsValid)
            {
                
                return View(model);
            }

            using (var db = new DBMVCEntities())
            {
                var oTipo = db.TIPDOCs.Find(model.Id);
                if (oTipo == null)
                {
                    
                    return HttpNotFound();
                }

                oTipo.TIPDOC1 = model.Tipodoc;
                oTipo.DESCRIPCION = model.Descripcion;
                oTipo.ORIGEN = model.Origen;
                oTipo.CTADEBITO = model.CtaDebito;
                oTipo.CTACREDITO = model.CtaCredito;

                db.Entry(oTipo).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction(Url.Content("~/TipdocQuery"));
        }
    }
}