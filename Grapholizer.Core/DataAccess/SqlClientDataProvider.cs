using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Grapholizer.Core.Utility;
using log4net;

namespace Grapholizer.Core.DataAccess
{
  public class SqlClientDataProvider : IDataProvider
  {
    static ILog Logger = LogManager.GetLogger(typeof(SqlClientDataProvider));


    #region Dependencies

    public IUnitOfWorkManager<SqlClientUnitOfWork> UnitOfWorkManager { get; set; }

    #endregion


    public DataTable Read(string sql, object parameters)
    {
      try
      {
        SqlCommand cmd = new SqlCommand(sql, UnitOfWorkManager.State.Connection);
        AddCommandParameters(cmd, parameters);

        SqlDataAdapter adapter = new SqlDataAdapter();
        adapter.SelectCommand = cmd;

        DataSet ds = new DataSet();
        adapter.Fill(ds, "data");

        return ds.Tables[0];
      }
      catch (SqlException ex)
      {
        Logger.Error(ex);
        Logger.InfoFormat("SQL: {0}", sql);
        throw;
      }
    }


    public DataRow ReadSingle(string sql, object parameters)
    {
      DataTable table = Read(sql, parameters);
      if (table.Rows.Count == 0)
      {
        Logger.DebugFormat("Could not find any rows for '{0}'", sql);
        throw new InvalidOperationException("No rows found");
      }
      return table.Rows[0];
    }


    protected void AddCommandParameters(SqlCommand cmd, object parameters)
    {
      if (parameters != null)
      {
        var dict = ReflectionHelper.ConvertObjectPropertiesToDictionary(parameters);

        foreach (KeyValuePair<string, object> item in dict)
        {
          if (item.Value is string)
          {
            SqlDbType type = ((string)item.Value).Length > 8000 ? SqlDbType.Text : SqlDbType.VarChar;
            cmd.Parameters.Add("@" + item.Key, type, ((string)item.Value).Length).Value = item.Value;
          }
          else
            cmd.Parameters.AddWithValue("@" + item.Key, item.Value);
        }
      }
    }


    public void Open()
    {
      if (UnitOfWorkManager.State.Connection.State == ConnectionState.Closed)
        UnitOfWorkManager.State.Connection.Open();
    }
  }
}
