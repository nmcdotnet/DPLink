using DipesLink.Views.SubWindows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static DipesLink.Views.Enums.ViewEnums;

namespace DipesLink.Views.Extension
{
    public class CusAlert
    {
        public static void Show(string message, ImageStyleMessageBox imgStyle, bool autoClose = false)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                AlertPopup cmb = new(message, imgStyle, autoClose);
                cmb.Show();
            });
        }
    }
}
