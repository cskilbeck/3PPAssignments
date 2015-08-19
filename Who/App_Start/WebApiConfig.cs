using System.Web.Http;

namespace Who
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "FindUsers",
                routeTemplate: "find/{controller}/{searchText}",
                defaults: new { controller="Users" }
            );
        }
    }
}