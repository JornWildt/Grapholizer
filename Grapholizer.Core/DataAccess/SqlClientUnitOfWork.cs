using System.Data.SqlClient;


namespace Grapholizer.Core.DataAccess
{
  public class SqlClientUnitOfWork
  {
    public SqlConnection Connection { get; set; }
  }
}
