using System.Web.Mvc;
using System.Web.Routing;

namespace PortalSicoobDivicred
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Login", action = "Login", id = UrlParameter.Optional}
            );
            routes.MapRoute(
                "Principal",
                "{controller}/{action}/{Acao}/{Mensagem}",
                new {controller = "Principal", action = "Principal", id = UrlParameter.Optional});
            routes.MapRoute(
                "PainelColaborador",
                "{controller}/{action}/",
                new { controller = "Principal", action = "Perfil", id = UrlParameter.Optional });
        }
    }
}