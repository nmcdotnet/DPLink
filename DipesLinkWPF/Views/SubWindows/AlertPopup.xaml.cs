using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using static DipesLink.Views.Enums.ViewEnums;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for AlertPopup.xaml
    /// </summary>
    public partial class AlertPopup : Window
    {
        private static List<AlertPopup> openPopups = new();
        DispatcherTimer? _timer;

        public AlertPopup(string message, ImageStyleMessageBox imgStyle, bool autoClose = false)
        {
            InitializeComponent();
            TextBoxMess.Text = message;
            switch (imgStyle)
            {
                case ImageStyleMessageBox.Info:
                    this.Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#0078D7"));
                    break;
                case ImageStyleMessageBox.Warning:
                    this.Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#FFA500"));
                    break;
                case ImageStyleMessageBox.Error:
                    this.Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#D8000C"));
                    break;
                default:
                    break;
            }


            ManagePopups(); // Handle popup limits

            this.ShowActivated = false;
            this.Topmost = true;

            if (autoClose)
            {
                _timer = new();
                _timer.Interval = TimeSpan.FromSeconds(5); // Popup lasts 5 seconds
                _timer.Tick += Timer_Tick;
                _timer.Start();
            }
        }


        private void ManagePopups()
        {
            
            if (openPopups.Count >= 5)
            {
                // Close the first popup in the list
                openPopups[0].Close();
                //openPopups.RemoveAt(0);
            }
           
            openPopups.Add(this); // Adds the current popup to the list
            UpdatePositions(); // Update location for all popups
        }

        private void SetPosition(int index, AlertPopup popup)
        {
            try
            {
                // Calculate the size and position of the screen
                double screenWidth = SystemParameters.WorkArea.Width;
                double screenHeight = SystemParameters.WorkArea.Height;
                popup.Left = screenWidth - popup.Width - 20; //Position on the right side of the screen
                popup.Top = screenHeight - popup.Height - (index * (popup.Height + 10)) - 20; //Stacking popups
            }
            catch (Exception) { }
        }

        private void UpdatePositions()
        {
            for (int i = 0; i < openPopups.Count; i++)
            {
                var text = openPopups[i].TextBoxMess.Text;
                SetPosition(i, openPopups[i]);
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            ((DispatcherTimer)sender)?.Stop(); //Stop timer
            this.Close(); // Close the popup window
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            openPopups.Remove(this);
            UpdatePositions(); //Update the position of remaining popups after a popup is closed
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            _timer?.Stop(); //Stop timer
            this.Close();
        }
    }
}
