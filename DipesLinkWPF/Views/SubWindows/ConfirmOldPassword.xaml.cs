using System.Windows;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for ConfirmOldPassword.xaml
    /// </summary>
    public partial class ConfirmOldPassword : Window
    {
        public bool IsConfirmed { get; set; } = false;
        public ConfirmOldPassword()
        {
            InitializeComponent();
        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = true;
            UsersManagement.OldPassword = PasswordBox1.Password;
            Close();
        }
    }
}
