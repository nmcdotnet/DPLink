using DipesLink.Views.SubWindows;
using static DipesLink.Views.Enums.ViewEnums;

namespace DipesLink.Views.Extension
{
    public class CusMsgBox
    {
        public static bool Show(string message, string caption,ButtonStyleMessageBox btnStyle, ImageStyleMessageBox imgStyle)
        {
                CustomMessageBox cmb = new(message, caption, btnStyle, imgStyle);
                if (cmb.ShowDialog() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }
    }
}
