using System;
using System.Configuration;
using System.Data.SqlClient;
using Grapholizer.Core.DataAccess;


namespace Grapholizer.WebApi.Utility
{
  public class WebUnitOfWorkManager : IUnitOfWorkManager<SqlClientUnitOfWork>, IDisposable
  {
    public SqlClientUnitOfWork State { get; protected set; }


    public WebUnitOfWorkManager()
    {
      string cs = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
      SqlConnection c = new SqlConnection(cs);
      State = new SqlClientUnitOfWork { Connection = c };
    }


    public void Dispose()
    {
      State.Connection.Close();
    }
  }
}