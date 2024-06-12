using Microsoft.VisualBasic.ApplicationServices;
using SharedProgram.Shared;
using static System.Windows.Forms.LinkLabel;
//using SQLite;
using SQLitePCL;
using SQLite;
using System.Windows.Controls;
using System.Windows;
using System.Transactions;
namespace RelationalDatabaseHelper.SQLite
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string username { get; set; } = "admin";
        public string password { get; set; } = "123456";
        public string role { get; set; } = "Administrator";
    }
    public class CreateDefaultUser
    {

        /// <summary>
        /// This Init will create a default User if not exist : thinhnguyen
        /// </summary>
        public static async void Init()
        {
            //Create Account Database Directory

            if (!Directory.Exists(SharedPaths.PathAccountsDb))
            {
                //var options = new SQLiteConnectionString(SharedPaths.PathAccountsDb, true, key: "password");


                Directory.CreateDirectory(SharedPaths.PathAccountsDb);

                // Get an absolute path to the database file

                var databasePath = Path.Combine(SharedPaths.PathAccountsDb, "MyData.db");
                SQLiteConnectionString options = new(databasePath, true, key: "123456");
                SQLiteAsyncConnection db = new(options);
                await db.CreateTableAsync<User>();
                User defaultUser = new()
                {
                    username = "admin",
                    password = "123456",
                    role = "Administrator",

                };

                await db.RunInTransactionAsync(tran => {
                    // database calls inside the transaction
                    tran.Insert(defaultUser);

                });
            }
        }


        private static void Transaction(SQLiteConnectionWithLock db, object data)
        {
            db.BeginTransaction();
            try
            {
                var t = db.Insert(data);
                db.Commit();

            }
            catch (Exception)
            {
                db.Rollback();
            }
        }
    }
}
