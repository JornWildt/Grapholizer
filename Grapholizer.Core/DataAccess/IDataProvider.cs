using System.Data;


namespace Grapholizer.Core.DataAccess
{
  public interface IDataProvider
  {
    DataTable Read(string sql, object parameters);
    DataRow ReadSingle(string sql, object parameters);
  }
}
