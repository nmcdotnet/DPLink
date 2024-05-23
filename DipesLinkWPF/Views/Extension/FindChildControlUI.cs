using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DipesLink.Views.Extension
{
    public static class FindChildControlUI
    {

        public static List<Grid> grids = new List<Grid>();
        /// <summary>
        /// Find Visual Child Control on UI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public  static void FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        { 
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T && ((FrameworkElement)child).Name == childName)
                {
                    grids.Add((Grid)child);
                }
                else
                {
                    FindVisualChild<T>(child, childName);
                }
            }

        }
    }
}
