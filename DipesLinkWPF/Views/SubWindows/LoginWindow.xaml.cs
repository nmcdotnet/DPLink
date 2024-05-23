using DipesLink.Extensions;
using DipesLink.Views.Extension;
using RelationalDatabaseHelper.SQLite;
using SharedProgram.Shared;
using System.Windows;
using System.Windows.Input;


namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool IsLoggedIn { get; set; }
        public string? Username { get; private set; }
        public string? Password { get; private set; }
        public bool IsRememberMe { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            CreateDefaultUser.Init();
            GetRememberLoginInfo();


        }
        private void GetRememberLoginInfo()
        {
            var info = SecureStorage.ReadCredentials();
            if (info.Username != null && info.Password != null)
            {
                TextBoxUsername.Text = info.Username;
                TextBoxPassword.Password = info.Password;
                if (info.Username != "" && info.Password != "")
                    CheckBoxRememberme.IsChecked = true;
            }
        }
        private void GetInputsValues()
        {
            Username = TextBoxUsername.Text;
            Password = TextBoxPassword.Password;
            if(CheckBoxRememberme.IsChecked!=null) IsRememberMe = (bool)CheckBoxRememberme.IsChecked;
        }

        private void LoginAction()
        {
            GetInputsValues();
            if (Username == null || Password == null) { return; }
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);
            sqlitehelper.OpenConnection();
            sqlitehelper.BeginTransaction();
            try
            {
                string commmand = @"SELECT * FROM users WHERE username = @username AND password = @password;";
                Dictionary<string, object> parameters = new()
                {
                    { "@username", Username },
                    { "@password", Password }
                };
                var user = sqlitehelper.ExecuteQuery(commmand, parameters).ToList().FirstOrDefault();
                if (user != null)
                {
                    user.TryGetValue("username", out var resUsername);
                    user.TryGetValue("password", out var resPassword);
                    user.TryGetValue("role", out var resRole);
                    if (resUsername != null) Application.Current.Properties["Username"] = resUsername.ToString();
                    if (resRole!=null) Application.Current.Properties["UserRole"] = AuthorizationHelper.GetRole(resRole); // save role
                    if (resUsername != null && resPassword! != null)
                    {
                        if (Username == resUsername.ToString() && Password == resPassword.ToString())
                        {
                            IsLoggedIn = true;
                            Hide();
                            if (IsRememberMe == true)
                            {
                                SecureStorage.SaveCredentials(Username, Password); // save file username and password
                            }
                            else
                            {
                                SecureStorage.SaveCredentials("", "");
                            }
                        }
                    }
                }
                else
                {
                    CusMsgBox.Show("Username or password is incorrect !", "Login", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Error);
                    //MessageBox.Show("Username not exist !");
                    //CusMsgBox.Show("Username or password is incorrect !","Login Fail",Enums.ViewEnums.ButtonStyleMessageBox.OK,Enums.ViewEnums.ImageStyleMessageBox.Error);
                }
            }
            catch (Exception)
            {
                sqlitehelper.RollbackTransaction();
                CusMsgBox.Show("Username or password is incorrect !", "Login", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Error);
                //CusMsgBox.Show("Username or password is incorrect !", "Login Fail", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Error);
                //MessageBox.Show("Login Fail");
            }
            finally
            {
                sqlitehelper.CloseConnection();
            }
        }

        
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void IconImage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginAction();
        }
    }
}
