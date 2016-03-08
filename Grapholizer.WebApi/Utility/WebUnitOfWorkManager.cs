using System.Web;
using Grapholizer.Core.DataAccess;


namespace Grapholizer.WebApi.Utility
{
  public class WebUnitOfWorkManager : IUnitOfWorkManager<SqlClientUnitOfWork>
  {
    internal const string UnitOfWorkStateName = "UnitOfWorkState";

    public SqlClientUnitOfWork State
    {
      get
      {
        return (SqlClientUnitOfWork)HttpContext.Current.Items[UnitOfWorkStateName];
      }
    }
  }
}