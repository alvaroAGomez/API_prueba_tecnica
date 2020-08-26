using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace webApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //enable Cors
            config.EnableCors(new EnableCorsAttribute("http://localhost:4200", headers:"*", methods:"*"));

            // Configuración y servicios de API web

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
