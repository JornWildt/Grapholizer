using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Grapholizer.Core.DataAccess;


namespace Grapholizer.WebApi.Utility
{
  public class WebUnitOfWorkHandler : DelegatingHandler
  {
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      try
      {
        string cs = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        SqlConnection c = new SqlConnection(cs);
        HttpContext.Current.Items[WebUnitOfWorkManager.UnitOfWorkStateName] = new SqlClientUnitOfWork { Connection = c };

        return await base.SendAsync(request, cancellationToken);
      }
      finally
      {
        if (HttpContext.Current.Items[WebUnitOfWorkManager.UnitOfWorkStateName] is SqlClientUnitOfWork)
        {
          ((SqlClientUnitOfWork)HttpContext.Current.Items[WebUnitOfWorkManager.UnitOfWorkStateName]).Connection.Dispose();
        }
      }
    }
  }
}