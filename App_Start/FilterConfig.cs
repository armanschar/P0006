using System.Web;
using System.Web.Mvc;

namespace P0006
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.VerificaSession()); // Agregamos el filtro de verificación de sesión
        }
    }
}
