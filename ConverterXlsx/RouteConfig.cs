using System.Web.Mvc;
using System.Web.Routing;

namespace ConverterXlsx
{
    /// <summary>
    /// Как по умолчанию разрешаются маршруты. Но вообще можно и задавать атрибутом Route в контроллере
    /// </summary>
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}