
//using Microsoft.Data.Sqlite;

using System.Data.SQLite;
using System.Data;
using SharedProgram.Shared;

namespace RelationalDatabaseHelper.SQLite
{
    public class SQLiteHelper : IDisposable
    {
        private readonly string _connectionString;
        private SQLiteConnection? _connection;
        private SQLiteTransaction? _transaction;
      

        public SQLiteHelper(string connectionString)
        {

            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = SharedPaths.PathAccountsDb +"\\"+ connectionString
            };

            _connectionString = builder.ToString();
        }

        public void OpenConnection()
        {
           
            try
            {
                _connection ??= new SQLiteConnection(_connectionString); // if null then create new
                                                                         // Set the password for new or existing database
             
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
            }
            catch (Exception) { }
        }
     
        public void CloseConnection()
        {
            try
            {
                if (_connection != null && _connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
            }
            catch (Exception) { }
        }

        public void BeginTransaction()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _transaction = _connection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        public int ExecuteNonQuery(string commandText, Dictionary<string, object>? parameters = null)
        {
            using var cmd = _connection?.CreateCommand();
            if (cmd == null) return 0;

            cmd.CommandText = commandText;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
            if (_transaction != null)
            {
                cmd.Transaction = _transaction;
            }
            return cmd.ExecuteNonQuery();
        }

        public IEnumerable<IDictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object>? parameters = null)
        {
            using var cmd = _connection?.CreateCommand();
            if (cmd != null)
            {
                cmd.CommandText = query;
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var result = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    yield return result; // return enum
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _transaction = null;
                CloseConnection();
                _connection?.Dispose();
                _connection = null;
            }
        }
    }
}
