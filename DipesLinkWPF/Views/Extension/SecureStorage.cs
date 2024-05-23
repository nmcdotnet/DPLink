using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DipesLink.Views.Extension
{
    public class SecureStorage
    {
        /// <summary>
        /// Encrypt username and password , then save as file
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void SaveCredentials(string username, string password)
        {
            byte[] encryptedUsername = ProtectedData.Protect(Encoding.UTF8.GetBytes(username), null, DataProtectionScope.CurrentUser);
            byte[] encryptedPassword = ProtectedData.Protect(Encoding.UTF8.GetBytes(password), null, DataProtectionScope.CurrentUser);

            File.WriteAllBytes("username.dat", encryptedUsername);
            File.WriteAllBytes("password.dat", encryptedPassword);
        }

        public static (string Username, string Password) ReadCredentials()
        {
            try
            {
                byte[] encryptedUsername = File.ReadAllBytes("username.dat");
                byte[] encryptedPassword = File.ReadAllBytes("password.dat");

                string username = Encoding.UTF8.GetString(ProtectedData.Unprotect(encryptedUsername, null, DataProtectionScope.CurrentUser));
                string password = Encoding.UTF8.GetString(ProtectedData.Unprotect(encryptedPassword, null, DataProtectionScope.CurrentUser));
                return (username, password);
            }
            catch (Exception)
            {
                return ("", "");
            }

        }
    }
}
