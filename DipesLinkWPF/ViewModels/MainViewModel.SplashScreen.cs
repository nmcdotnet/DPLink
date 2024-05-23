using DipesLink.Views.SubWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DipesLink.ViewModels
{
    public partial class MainViewModel
    {
        //public SplashScreenLoading? SplashScreenLoading;
        private void SplashScreenLoadingAction(string actionString, SplashScreenLoading? window)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {

                if (actionString == "show")
                {
                    //Application.Current.Dispatcher.Invoke(() =>
                    //{
                    window?.Show();
                    // });
                }
                else
                {
                    if (window != null)
                    {
                        window.Close();
                        window = null;
                    }
                }

            });


        }

    }
}
