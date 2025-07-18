using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P0006.Controllers
{
    public class CerrarSessionController : Controller
    {
        // GET: CerrarSession
        public ActionResult Logoff()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceder");
        }
    }
}