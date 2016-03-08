using System;
using System.Reflection;
using System.Web.Http;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Grapholizer.Core;
using Grapholizer.Core.DataAccess;
using Grapholizer.WebApi.Utility;
using log4net;

namespace Grapholizer.WebApi
{
  public static class WebApiConfig
  {
    static ILog Logger = LogManager.GetLogger(typeof(WebApiConfig));


    public static void Register(HttpConfiguration config)
    {
      log4net.Config.XmlConfigurator.Configure();
      Logger.Info("****** Starting Grapholizer API ******");

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
      ConfigureDependencies(config);
      config.MessageHandlers.Add(new WebUnitOfWorkHandler());
    }


    private static void ConfigureDependencies(HttpConfiguration config)
    {
      var kernel = new DefaultKernel();

      // Register known services
      kernel.Register(Component.For<IUnitOfWorkManager<SqlClientUnitOfWork>>().ImplementedBy<WebUnitOfWorkManager>());
      kernel.Register(Component.For<IDataProvider>().ImplementedBy<SqlClientDataProvider>());
      kernel.Register(Component.For<GraphService>().ImplementedBy<GraphService>());

      // Register all API controllers
      foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
      {
        if (typeof(ApiController).IsAssignableFrom(t))
          kernel.Register(Component.For(t).ImplementedBy(t).LifeStyle.Is(Castle.Core.LifestyleType.PerWebRequest));
      }

      // Register dependency resolver
      config.DependencyResolver = new CastleWindsorDependencyResolver(kernel);
    }
  }
}
