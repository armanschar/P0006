using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P0006.Models;

namespace P0006.Controllers
{
    public class AccederController : Controller
    {
        // GET: Acceder
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Enter(string usuario, string password)
        {
            using (DBMVCEntities db = new DBMVCEntities())
            {
                //SELECT * FROM USERS WHERE Email = usuario AND Password = password AND idEstatus = 1
                var read = from d in db.USERS
                            where d.Email == usuario 
                            && d.Password == password
                            && d.idEstatus == 1
                            select d;
                if (read.Count() > 0) //pregunta si encontro el registro
                {
                    Session["usuario"] = read.First(); //creando e iniciando sesion
                    return Content("1"); //devuelve un 1 a la vista indicando que se encontro un registro
                }
                else
                {
                    return Content("El usuario y/o la contraseña son incorrectos"); //si no hay registro entrega una alerta
                }
            }
        }
    }
}