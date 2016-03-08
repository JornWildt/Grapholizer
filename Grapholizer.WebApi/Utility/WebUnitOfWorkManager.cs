using System;
using System.Data.SqlClient;
using Grapholizer.Core.DataAccess;


namespace Grapholizer.WebApi.Utility
{
  public class WebUnitOfWorkManager : IUnitOfWorkManager<SqlClientUnitOfWork>, IDisposable
  {
    public SqlClientUnitOfWork State { get; protected set; }


    public WebUnitOfWorkManager()
    {
      SqlConnection c = new SqlConnection("");
      State = new SqlClientUnitOfWork { Connection = c };
    }


    public void Dispose()
    {
      State.Connection.Close();
    }
  }
}