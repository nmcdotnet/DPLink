using DipesLink.Models;
using DipesLink.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DipesLink.Views.UserControls.MainUc
{
    /// <summary>
    /// Interaction logic for OverviewJobUc.xaml
    /// </summary>
    public partial class OverviewJobUc : UserControl
    {
        public JobOverview ViewModel
        {
            get { return (JobOverview)DataContext; }
            set { DataContext = value; }
        }
        public OverviewJobUc()
        {
            InitializeComponent();
            this.Loaded += OverviewJobUc_Loaded;
        }

        private void OverviewJobUc_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
    }
}
