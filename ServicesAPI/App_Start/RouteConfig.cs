using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ServicesAPI
{
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

            //routes.MapRoute(
            //    name: "GetSeatPlan",
            //    url: "{controller}/GetSeatPlan",
            //    defaults: new { controller = "ReelCinemas" }
            //);

            //routes.MapRoute(
            //    name: "GetTickets",
            //    url: "{controller}/GetTickets",
            //    defaults: new { controller = "ReelCinemas" }
            //);
        }
    }
}
