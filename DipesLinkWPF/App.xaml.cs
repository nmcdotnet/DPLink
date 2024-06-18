using DipesLink.Views;
using DipesLink.Views.SubWindows;
using SharedProgram.Shared;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace DipesLink
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SQLitePCL.Batteries_V2.Init();
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            //this.Icon = new Icon("pack://application:,,,/DipesLink;component/Images/Duplicated_res.png");


            var loginWindow = new LoginWindow();
            //MainWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/DipesLink;component/Images/statistic_1.png"));
            loginWindow.ShowDialog();
            if (loginWindow.IsLoggedIn)
            {
                try
                {
                    var mainWindow = new MainWindow();
                    MainWindow = mainWindow;
                    MainWindow.Show();
                    loginWindow.Close();
                }
                catch (Exception)
                {
                    Debug.Write("Login Failed!");
                }
            }
            else
            {
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            ShutdownProcess();
        }

        private static void ShutdownProcess()
        {
            foreach (Process process in Process.GetProcessesByName($"{SharedValues.DeviceTransferName}"))
            {
                process.Kill();
            }
            foreach (Process process in Process.GetProcessesByName($"DipesLink"))
            {
                process.Kill();
            }
        }
    }
}
