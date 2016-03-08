using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Grapholizer.Core.DataAccess;
using Grapholizer.WebApi.Utility;

namespace Grapholizer.WebApi
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = RouteParameter.Optional });

      config.Routes.MapHttpRoute(
        name: "GraphApi",
        routeTemplate: "graph/{name}/{node}/{id}",
        defaults: new { controller = "graph" });

      config.Formatters.Remove(config.Formatters.XmlFormatter);

      var kernel = new DefaultKernel();
      kernel.Register(Component.For<IDataProvider>().ImplementedBy<SqlClientDataProvider>());
      foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (typeof(ApiController).IsAssignableFrom(t))
          kernel.Register(Component.For(t).ImplementedBy(t).LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));
      }
      config.DependencyResolver = new CastleWindsorDependencyResolver(kernel);
    }
  }
}
