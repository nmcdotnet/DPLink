using DipesLink.Views.SubWindows;
using System.Windows;

namespace DipesLink.Views.Extension
{
    public class SplashScreenCus
    {
        public static SplashScreenCus StartNew() => new();
        
        private SplashScreenLoading spl;
        public SplashScreenCus()
        {
            spl = new SplashScreenLoading();
        }
        public void Show()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                spl.Show();
            });
        }
        public void Close()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                spl.Close();
            });
        }
    }
}
