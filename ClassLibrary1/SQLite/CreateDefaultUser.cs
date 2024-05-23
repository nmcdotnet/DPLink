using SharedProgram.Shared;

namespace RelationalDatabaseHelper.SQLite
{
    public class CreateDefaultUser
    {
        public static void Init()
        {
            //Create Account Database Directory
            if (!Directory.Exists(SharedPaths.PathAccountsDb))
            {
                Directory.CreateDirectory(SharedPaths.PathAccountsDb);
            }
            InitTable();
            CreateUser();
        }

        private static void InitTable()
        {
            // Batteries_V2.Init();
           
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);

            sqlitehelper.OpenConnection();
           
            sqlitehelper.BeginTransaction();

            try
            {
                string commandCreateTable = @"
                    CREATE TABLE IF NOT EXISTS users (
                      id INTEGER PRIMARY KEY AUTOINCREMENT,
                      username TEXT NOT NULL UNIQUE,
                      password TEXT NOT NULL,
                      role TEXT NOT NULL CHECK (role IN ('Administrator', 'Operator'))
                    );
                    ";

                sqlitehelper.ExecuteNonQuery(commandCreateTable);
                sqlitehelper.CommitTransaction();


            }
            catch (Exception)
            {
                sqlitehelper.RollbackTransaction();
            }
            finally
            {
                sqlitehelper.CloseConnection();
            }
        }
        private static void CreateUser()
        {
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);
            sqlitehelper.OpenConnection();
            sqlitehelper.BeginTransaction();
            try
            {
                string createUserCommand =
                    @"INSERT OR IGNORE INTO users (username,password, role) VALUES (@username, @password, @role)";
                Dictionary<string, object> parameters = new()
                {
                    { "@username", "admin" },
                    { "@password", "123456" },
                    { "@role",   "Administrator" }
                };
                sqlitehelper.ExecuteNonQuery(createUserCommand, parameters);
                sqlitehelper.CommitTransaction();
            }
            catch (Exception)
            {
                sqlitehelper.RollbackTransaction();
            }
            finally
            {
                sqlitehelper.CloseConnection();
            }
        }
    }
}
