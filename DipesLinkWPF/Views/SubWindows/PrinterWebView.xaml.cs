using System.Windows;
using System.Windows.Controls;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for PrinterWebView.xaml
    /// </summary>
    public partial class PrinterWebView : Window
    {
        public string? Address { get; set; }
        public string? TitleContext { get; set; }

        public PrinterWebView()
        {
            InitializeComponent();
            
            this.Loaded += PrinterWebView_Loaded;
        }

        public void SetWindowTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                base.Title = "Default Title";
            }
            else
            {
                base.Title = title;
            }
        }

        private void PrinterWebView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetWindowTitle(TitleContext);
                var address = $"http://{Address}";
                webView.Source = new Uri(address);
            }
            catch (Exception)
            {
                var address = $"http://google.com";
                webView.Source = new Uri(address);
            } 
        }
    }
}
