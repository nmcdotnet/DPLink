using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DipesLink.Views.CustomDialogs
{
    /// <summary>
    /// Interaction logic for MessageBoxCus.xaml
    /// </summary>
    public partial class MessageBoxCus : Window
    {
        public string Message { get; set; }
        public ICommand OkCommand { get; set; }
        public MessageBoxCus(string title,string message)
        {
            InitializeComponent();
            Title = title;
            Message = message;
            OkCommand = new RelayCommand(OkAction);
            DataContext = this;
        }

        private void OkAction()
        {
            DialogResult = true;
        }
    }
}
