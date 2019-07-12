using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YxLiCai.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Home1", "Home/Index", new { controller = "Sys", action = "Index" });
            routes.MapRoute("Home2", "Home", new { controller = "Sys", action = "Index" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Sys", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}