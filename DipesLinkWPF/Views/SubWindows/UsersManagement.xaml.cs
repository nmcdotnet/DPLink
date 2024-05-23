using DipesLink.Models;
using DipesLink.Views.Extension;
using RelationalDatabaseHelper.SQLite;
using SharedProgram.Shared;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for UsersManagement.xaml
    /// </summary>
    public partial class UsersManagement : Window
    {
        public static string OldPassword = string.Empty;
        bool isInit = true;
        public UsersManagement()
        {
            InitializeComponent();
            isInit = true;  
            Loaded += UsersManagement_Loaded;
        }

        private void UsersManagement_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllUser();
            isInit = false;
        }

        private void LoadAllUser()
        {
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);
            sqlitehelper.OpenConnection();
            sqlitehelper.BeginTransaction();
            try
            {
                string commmand = @"SELECT * FROM users";
                List<IDictionary<string, object>> usersList = sqlitehelper.ExecuteQuery(commmand).ToList();
                List<UsersModel> listDataUsers = ConvertToListOfStringArrays(usersList);
                DataGridUsers.ItemsSource = listDataUsers;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static List<UsersModel> ConvertToListOfStringArrays(List<IDictionary<string, object>> listDict)
        {
            List<UsersModel> resultList = new();

            foreach (var dict in listDict)
            {
                string[] array = new string[dict.Count];
                UsersModel user = new UsersModel();
                int i = 0;
                foreach (var pair in dict)
                {
                    i++;
                    if (pair.Key == "id")
                        user.Id = pair.Value.ToString();
                    if (pair.Key == "username")
                        user.Username = pair.Value.ToString();
                    if (pair.Key == "password")
                        user.Password = pair.Value.ToString();
                    if (pair.Key == "role")
                        user.Role = pair.Value.ToString();

                }
                resultList.Add(user);
            }

            return resultList;
        }

        private bool CheckInvalidInput()
        {
            // Regex for username
            //Must be 8-20 characters long.
            //Can include letters, numbers, and underscores.
            //Must start with a letter.

            //Regex for password
            //At least 8 characters long.
            //Must contain at least one uppercase letter.
            //Must contain at least one lowercase letter.
            //Must contain at least one digit.
            //Optionally, include special characters for stronger security.

            Regex usernameRegex = new Regex(@"^[a-zA-Z]\w{7,19}$");
            Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d@#$%^&+=]{8,}$");

            if (TextBoxUsername.Text != null)
            {
                bool isUsernameValid = usernameRegex.IsMatch(TextBoxUsername.Text);
                if (!isUsernameValid)
                {
                    CusMsgBox.Show("Username Invalid", "Input Validation", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Warning);
                    return false;
                }
            }
            if (TextBoxPassword.Text == null)
            {
                //bool isPasswordValid = passwordRegex.IsMatch(TextBoxPassword.Text);
                //if (!isPasswordValid)
                //{
                CusMsgBox.Show("Username Invalid", "Input Validation", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Warning);
                //}
                return false;
            }
            return true;

        }
        private void CreateUser()
        {
            bool isCreated = false;
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);
            sqlitehelper.OpenConnection();
            sqlitehelper.BeginTransaction();
            try
            {
                string createUserCommand =
                    @"INSERT INTO users (username,password, role) VALUES (@username, @password, @role)";
                Dictionary<string, object> parameters = new()
                {
                    { "@username", TextBoxUsername.Text },
                    { "@password", TextBoxPassword.Text },
                    { "@role",   ((ComboBoxItem)ComboBoxRole.SelectedItem).Content.ToString()  }
                };
                sqlitehelper.ExecuteNonQuery(createUserCommand, parameters);
                sqlitehelper.CommitTransaction();
                isCreated = true;
            }
            catch (Exception)
            {
                isCreated = false;
                sqlitehelper.RollbackTransaction();
                CusMsgBox.Show("Create User Failed !", "Create User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Error);
            }
            finally
            {
                if (isCreated)
                {
                    CusMsgBox.Show("Create User Done !", "Create User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Info);
                }
                sqlitehelper.CloseConnection();
            }
        }

        private void EditUserPassword(string username, string newPassword, string oldPassword, string newRole)
        {
            bool isChanged = false;
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);
            sqlitehelper.OpenConnection();
            sqlitehelper.BeginTransaction();

            try
            {
                string editUserPassword = @"UPDATE users SET password = @newPassword, role = @newRole WHERE username = @username AND password = @oldPassword";
                Dictionary<string, object> parameters = new()
                {
                    { "@username", username },
                    { "@newPassword", newPassword },
                    { "@oldPassword", oldPassword },
                    { "@newRole", newRole }
                };
                int rowCount = sqlitehelper.ExecuteNonQuery(editUserPassword, parameters);
                if (rowCount >= 1) { isChanged = true;  } else throw new Exception("Not row effected");
                sqlitehelper.CommitTransaction();
                
            }
            catch (Exception)
            {
                isChanged = false;
                sqlitehelper.RollbackTransaction();
                CusMsgBox.Show("Change user info failed !", "Edit User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Error);
            }
            finally
            {
                if (isChanged)
                {
                    CusMsgBox.Show($"Change info for {username} done !", "Edit User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Info);
                }
                sqlitehelper.CloseConnection();
            }

        }

        private bool CheckCurrentUser(string username)
        {
            string curUsername = Application.Current.Properties["Username"].ToString();
            return curUsername == username;
        }
        private void DeleteUser(string username)
        {
            bool isDeleted = false;
            using SQLiteHelper sqlitehelper = new(SharedValues.ConnectionString);
            sqlitehelper.OpenConnection();
            sqlitehelper.BeginTransaction();

            try
            {
                string deleteUserCommand = @"DELETE FROM users WHERE username = @username";
                Dictionary<string, object> parameters = new()
                {
                    { "@username", username }
                };

                sqlitehelper.ExecuteNonQuery(deleteUserCommand, parameters);
                sqlitehelper.CommitTransaction();
                isDeleted = true;
            }
            catch (Exception)
            {
                isDeleted = false;
                sqlitehelper.RollbackTransaction();
                CusMsgBox.Show("Delete User Failed !", "Delete User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Error);
            }
            finally
            {
                if (isDeleted)
                {
                    CusMsgBox.Show("Delete User Done !", "Delete User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Info);
                }
                sqlitehelper.CloseConnection();
            }
        }
      

  

        private void Red_Checked(object sender, RoutedEventArgs e)
        {
            if (isInit == true) { return; };
            var rad = sender as RadioButton;
            switch (rad.Name)
            {
                case "RadNew":
                    TextBoxPassword.IsEnabled = true;
                    ComboBoxRole.IsEnabled = true;
                    break;
                case "RadEdit":
                    TextBoxPassword.IsEnabled = true;
                    ComboBoxRole.IsEnabled = true;
                    break;
                case "RadDel":
                    TextBoxPassword.IsEnabled = false;
                    ComboBoxRole.IsEnabled = false; 
                    break;
                default:
                    break;
            }
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {

            //ADD NEW
            if(RadNew.IsChecked == true)
            {
                if (CheckInvalidInput())
                {
                    var res = CusMsgBox.Show("Create new user ?","Create new user", Enums.ViewEnums.ButtonStyleMessageBox.OKCancel,Enums.ViewEnums.ImageStyleMessageBox.Info);
                    if (res)
                    {
                        CreateUser();
                        LoadAllUser();
                    }
                }
            }

            //EDIT
            if (RadEdit.IsChecked == true)
            {
                ConfirmOldPassword cfmOldPassWindow = new();
                var res = cfmOldPassWindow.ShowDialog();
                if (cfmOldPassWindow.IsConfirmed)
                {
                    EditUserPassword(TextBoxUsername.Text, TextBoxPassword.Text, OldPassword, ((ComboBoxItem)ComboBoxRole.SelectedItem).Content.ToString());
                }
            }

            //DELETE
            if (RadDel.IsChecked == true)
            {
                string inputUsername = TextBoxUsername.Text;
                if (!CheckCurrentUser(inputUsername))
                {
                    var res = CusMsgBox.Show($"Delete user: {inputUsername} ?", "Delete User", Enums.ViewEnums.ButtonStyleMessageBox.OKCancel, Enums.ViewEnums.ImageStyleMessageBox.Info);
                    if (res)
                    {
                        DeleteUser(inputUsername);
                        LoadAllUser();
                    }
                }
                else
                {
                    CusMsgBox.Show("Can not delete current user!", "Delete User", Enums.ViewEnums.ButtonStyleMessageBox.OK, Enums.ViewEnums.ImageStyleMessageBox.Warning);
                }
            }
        }

        private void DataGridUsers_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DataGridUsers.SelectedItem != null)
            {
                var item = DataGridUsers.SelectedItem; // This is the row data.
                                                  // Assuming the model has a property named 'Name'
                var username = item.GetType().GetProperty("Username").GetValue(item, null);
                TextBoxUsername.Text = username.ToString();
                var role = item.GetType().GetProperty("Role").GetValue(item, null);
                if (role.ToString()=="Administrator")
                {
                    ComboBoxRole.SelectedIndex =0;
                }
                else
                {
                    ComboBoxRole.SelectedIndex = 1;
                }
            }
        }
    }
}
