using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Baze3.Database
{
    public sealed class SqlDatabase : IDatabase
    {
        public string ConnectionString { get; private set; }

        public SqlDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IEnumerable<IDataRecord> Query(string sql, Action<SqlParameterCollection> bind = null)
        {
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (bind != null) bind(cmd.Parameters);

                con.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read()) yield return rdr;
                }
            }
        }

        public int Execute(string sql, Action<SqlParameterCollection> bind = null)
        {
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (bind != null) bind(cmd.Parameters);

                con.Open();
                
                return cmd.ExecuteNonQuery();
            }
        }

        public T Scalar<T>(string sql, Action<SqlParameterCollection> bind = null)
        {
            using (var con = new SqlConnection(ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                if (bind != null) bind(cmd.Parameters);

                con.Open();
                object obj = cmd.ExecuteScalar();

                if (obj == null || obj is DBNull) return default(T);

                return (T)Convert.ChangeType(obj, typeof(T));
            }
        }

        public void Transaction(Action<SqlConnection, SqlTransaction> work)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (var tx = con.BeginTransaction())
                {
                    try { work(con, tx); tx.Commit(); }
                    catch { tx.Rollback(); throw; }
                }
            }
        }
    }
}
