using DipesLink.Extensions;
using DipesLink.Models;
using DipesLink.ViewModels;
using DipesLink.Views.Extension;
using DipesLink.Views.SubWindows;
using DipesLink.Views.UserControls.MainUc;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Windows;
using DipesLink.Views.Models;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Application = System.Windows.Application;
using System;

namespace DipesLink.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static event EventHandler<EventArgs>? MainWindowSizeChangeCustomEvent;
        public static SplashScreenLoading? SplashScreenLoading = new();
        public static event EventHandler? ListBoxMenuSelectionChange;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainViewModel.GetIntance();
            EventRegister();
            if (AuthorizationHelper.IsAdmin())
            {
                ComboBoxSelectView.IsEnabled = false;
            }
        }

        private void EventRegister()
        {
            JobDetails.OnJobDetailChange += JobDetails_OnJobDetailChange;
            SizeChanged += MainWindow_SizeChanged;
            Closing += MainWindow_Closing;
            Closed += MainWindow_Closed;
            AllStationUc.DoneLoadUIEvent += AllStationUc_DoneLoadUIEvent;
            Shared.OnActionLoadingSplashScreen += Shared_OnActionLoadingSplashScreen;
            ListBoxMenu.SelectionChanged += ListBoxMenu_SelectionChanged;
        }

        private void JobDetails_OnJobDetailChange(object? sender, int e)
        {
            var CamIP = ViewModelSharedValues.Settings.SystemParamsList[e].CameraIP;
            var PrinterIP = ViewModelSharedValues.Settings.SystemParamsList[e].PrinterIP;
            var ControllerIP = ViewModelSharedValues.Settings.SystemParamsList[e].ControllerIP;
            TextBlockControllerIP.Text = ControllerIP.ToString();
            TextBlockPrinterIP.Text = PrinterIP.ToString();
            TextBlockCamIP.Text= CamIP.ToString();
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            var isExit = CusMsgBox.Show("Do you want to exit the application ?", "Exit Application", Enums.ViewEnums.ButtonStyleMessageBox.YesNo, Enums.ViewEnums.ImageStyleMessageBox.Warning);
            if (!isExit)
            {
                e.Cancel = true;
            }
        }

        private void ListBoxMenu_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ListBoxMenuSelectionChange?.Invoke(ListBoxMenu.SelectedIndex, EventArgs.Empty);
            if (ListBoxMenu.SelectedIndex != -1)
                ComboBoxSelectView.SelectedIndex = 0;
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current?.Shutdown();
        }

        private void AllStationUc_DoneLoadUIEvent(object? sender, EventArgs e)
        {
            MainWindowSizeChangeCustomEvent?.Invoke(ActualWidth, EventArgs.Empty);
        }

        private void Shared_OnActionLoadingSplashScreen(object? sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender == null) return;
                var actionType = (DataTypes.ActionSplashScreen)sender;

                if (actionType == DataTypes.ActionSplashScreen.Show)
                {
                    SplashScreenLoading ??= new SplashScreenLoading();
                    SplashScreenLoading.ShowDialog();
                }
                else
                {
                    if (SplashScreenLoading != null)
                    {
                        SplashScreenLoading.Close();
                        SplashScreenLoading = null;
                    }
                }
            });
        }

        public void CallbackCommand(Action<MainViewModel> execute)
        {
            try
            {
                if (DataContext is MainViewModel model)
                {
                    execute?.Invoke(model);
                }
                else
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private T? CurrentViewModel<T>() where T : class
        {
            if (DataContext is T viewModel)
            {
                return viewModel;
            }
            else
            {
                return null;
            }
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double actualWidth = ActualWidth;
            //MainViewModel viewModel = (MainViewModel)DataContext;
            //viewModel.ActualWidthMainWindow = actualWidth;
            //CallbackCommand(vm => vm.MinusWidth());
            MainWindowSizeChangeCustomEvent?.Invoke(ActualWidth, EventArgs.Empty);
        }

        private void ButtonExitApp_Click(object sender, RoutedEventArgs e)
        {
            var isExit = CusMsgBox.Show("Do you want to exit the application ?", "Exit Application", Enums.ViewEnums.ButtonStyleMessageBox.YesNo, Enums.ViewEnums.ImageStyleMessageBox.Warning);
            if (isExit)
            {
                Application.Current?.Shutdown();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeviceStatListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var index = DeviceStatListBox.SelectedIndex;
            CallbackCommand(vm => vm.ChangeJobByDeviceStatSymbol(index));
        }

        private void ComboBoxStationNum_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //CurrentViewModel<MainViewModel>().CheckStationChange();
        }

        private void ComboBoxSelectView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var cbb = (System.Windows.Controls.ComboBox)sender;
            if (cbb.SelectedIndex == 1)
            {
                ListBoxMenu.SelectedIndex = -1;
            }
            else
            {
                ListBoxMenu.SelectedIndex = 0;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                switch (menuItem.Header)
                {
                    case "Account Management":
                        UsersManagement um = new();
                        um.Show();
                        break;
                    case "Logout": //Restart
                        var res = CusMsgBox.Show("Do you want to logout ?", "Logout", Enums.ViewEnums.ButtonStyleMessageBox.OKCancel, Enums.ViewEnums.ImageStyleMessageBox.Warning);
                        if (res)
                        {
                            Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                            Application.Current.Shutdown();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void BorderUser_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Find the button that was clicked
            var userUI = sender as Border;
            if (userUI != null)
            {
                // Find the ContextMenu resource
                ContextMenu contextMenu = this.FindResource("MyContextMenu") as ContextMenu;
                if (contextMenu != null)
                {
                    foreach (var item in contextMenu.Items)
                    {
                        if (item is MenuItem menuItem)
                        {
                            menuItem.Click -= MenuItem_Click;
                            menuItem.Click += MenuItem_Click;
                        }
                    }


                    // Set the placement target and open the context menu
                    contextMenu.PlacementTarget = userUI;
                    contextMenu.IsOpen = true;

                    TextBlock adminTextBlock = contextMenu.Template.FindName("UsernameTextBlock", contextMenu) as TextBlock;
                    if (adminTextBlock != null)
                    {
                        adminTextBlock.Text = (string)Application.Current.Properties["Username"]; // Thay đổi nội dung
                    }
                }
            }
        }
        private void ChangeView()
        {
          
            ListBoxMenu.SelectedIndex = ListBoxMenu.SelectedIndex != -1 ? -1 : 0;
            if (ListBoxMenu.SelectedIndex != -1)
            {
                IpGridChange.Visibility = Visibility.Visible;
                myToggleButton.IsChecked = false; // This will trigger the setter for True

            }
            else
            {
                // or
                IpGridChange.Visibility = Visibility.Hidden;
                myToggleButton.IsChecked = true; // This will trigger the setter for False
  
            }
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            
            ChangeView();
        }

       

        private void ListBoxItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myToggleButton.IsChecked = false;
            IpGridChange.Visibility = Visibility.Visible;

        }
    }
}