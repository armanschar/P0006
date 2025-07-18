using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P0006.Models.ViewModels;
using P0006.Models;

namespace P0006.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Query()
        {
            List<QueryViewModels> lst = null;

            using (DBMVCEntities db = new DBMVCEntities())
            {
                //realizamos la lectura a la tabla con linq
                lst = (from d in db.USERS
                       where d.idEstatus == 1
                       orderby d.Email

                       //llenamos el modelo que se llama QueryViewModels
                       select new QueryViewModels
                       {
                           _Id = d.ID, //asigna los valores a cada columna del mdoelo
                           _Email = d.Email,
                           _Edad = d.Edad
                       }).ToList();
            }
            return View(lst); //enviamos la data hacia la vista llamada Query
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View(); //muestra la vista vacia
        }
        [HttpPost]
        public ActionResult Add(AddUserViewModels model)
        {
            //valida que la informacion que viene de la vista sea correcta
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //abre la base de datos
            using (var db = new DBMVCEntities())
            {

                USER oUser = new USER(); //abre la tabla en modo INSERT INTO

                //movemos la data del modeloo hacia la clase que representa la tabla
                oUser.Nombre = model.Nombre;
                oUser.Usuario = model.Usuario;
                oUser.Email = model.Email;
                oUser.Password = model.Password;
                oUser.Edad = model.Edad;
                oUser.idEstatus = 1;
                db.USERS.Add(oUser); //en este punto solo se guarda en memoria
                db.SaveChanges(); //guardamos los cambios en la tabla
            }

            return RedirectToAction(Url.Content("~/Query")); //muestra la consulta con el nuevo registro
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            EditUserViewModels model = new EditUserViewModels();
            using (var db = new DBMVCEntities())
            {
                var oUser = db.USERS.Find(Id);

                model.Usuario = oUser.Usuario;
                model.Edad = oUser.Edad;
                model.Email = oUser.Email;
                model.Password = oUser.Password;
                model.Id = oUser.ID;

            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModels model)
        {
            if (!ModelState.IsValid)
            {
                //valida que la informacion que viene de la vista sea correcta
                return View(model);
            }

            using (var db = new DBMVCEntities())
            {
                var oUser = db.USERS.Find(model.Id);
                if (oUser == null)
                {
                    // si no aparece el usuario, retorna un error 404
                    return HttpNotFound();
                }

                oUser.Email = model.Email;
                oUser.Edad = model.Edad;

                // solo actualiza la contraseña si se ha proporcionado una nueva y no está vacía
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    oUser.Password = model.Password;
                }

                db.Entry(oUser).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction(Url.Content("~/Query")); 
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            using (var db = new DBMVCEntities())
            {
                var oUser = db.USERS.Find(Id);
                if (oUser == null)
                    return Content("0"); // en caso de eror, es por que el usuario no fue encontrado

                oUser.idEstatus = 3;
                db.Entry(oUser).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }
            return Content("1"); // se encontró y se eliminó correctamente
        }
    }
}