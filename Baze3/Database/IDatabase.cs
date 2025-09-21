using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Baze3.Database
{
    public interface IDatabase
    {
        string ConnectionString { get; }
        IEnumerable<IDataRecord> Query(string sql, Action<SqlParameterCollection> bind = null);
        int Execute(string sql, Action<SqlParameterCollection> bind = null);
        T Scalar<T>(string sql, Action<SqlParameterCollection> bind = null);
        void Transaction(Action<SqlConnection, SqlTransaction> work);
    }
}
