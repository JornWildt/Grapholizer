using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Castle.MicroKernel;

namespace Grapholizer.WebApi.Utility
{
  public class CastleWindsorDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver
  {
    private readonly IKernel Kernel;


    public CastleWindsorDependencyResolver(IKernel kernel)
    {
      Kernel = kernel;
    }


    public IDependencyScope BeginScope()
    {
      IKernel child = new DefaultKernel();
      Kernel.AddChildKernel(child);
      return new CastleWindsorDependencyResolver(child);
    }

    public void Dispose()
    {
      if (Kernel.Parent != null)
        Kernel.Parent.RemoveChildKernel(Kernel);
      Kernel.Dispose();
    }

    public object GetService(Type serviceType)
    {
      try
      {
        return Kernel.Resolve(serviceType);
      }
      catch (ComponentNotFoundException)
      {
        return null;
      }
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
      try
      {
        return Kernel.ResolveAll(serviceType).Cast<object>();
      }
      catch (ComponentNotFoundException)
      {
        return new object[0];
      }
    }
  }
}