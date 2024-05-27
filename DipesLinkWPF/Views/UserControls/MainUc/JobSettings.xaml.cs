using DipesLink.ViewModels;
using DipesLink.Views.SubWindows;
using DipesLink.Views.UserControls.CustomControl;
using SharedProgram.Models;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using TextBox = System.Windows.Controls.TextBox;

namespace DipesLink.Views.UserControls.MainUc
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class JobSettings : UserControl
    {
        public static bool IsInitializing = true;
        public JobSettings()
        {
            InitializeComponent();
            Loaded += SettingsUc_Loaded;
            TextBoxPrinterIP.TextChanged += TextBox_ParamsChanged;
            TextBoxCamIP.TextChanged += TextBox_ParamsChanged;
            TextBoxControllerIP.TextChanged += TextBox_ParamsChanged;
        }
      


        private void SettingsUc_Loaded(object sender, RoutedEventArgs e)
        {
            IsInitializing = false;
            var vm = CurrentViewModel<MainViewModel>();
            vm?.SaveConnectionSetting();
           
            RadBasic.IsChecked = vm?.ConnectParamsList[CurrentIndex()].VerifyAndPrintBasicSentMethod;
        }

       
        //public void CallbackCommand(Action<MainViewModel> execute)
        //{
        //    try
        //    {
        //        if (DataContext is MainViewModel model)
        //        {
        //            execute?.Invoke(model);
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }
        //}

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
       

        private void ListBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentViewModel<MainViewModel>()?.SelectionChangeSystemSettings(CurrentIndex());
        }

        private int CurrentIndex() => ListBoxMenu.SelectedIndex;


        private void TextBox_ParamsChanged(object sender, TextChangedEventArgs e)
        {
           
            if (IsInitializing) return;
            if(sender is IpAddressControl textBoxIp)
            {
                TextBoxIpParamsHandler(textBoxIp);
            }
            if (sender is TextBox textBox)
            {
                TextBoxNormalParamsHandler(textBox);
            }
        }

        private void TextBoxIpParamsHandler(IpAddressControl textBox)
        {
            textBox?.Dispatcher.BeginInvoke(new Action(() => // Use BeginInvoke to Update Input Last Value 
            {
                var vm = CurrentViewModel<MainViewModel>();
                if (vm == null) return;
                if (vm.ConnectParamsList == null) return;
                textBox.Text ??= string.Empty;
                switch (textBox.Name)
                {
                    case "TextBoxPrinterIP":
                        vm.ConnectParamsList[CurrentIndex()].PrinterIP = textBox.Text;
                        break;
                    case "TextBoxPrinterPort":
                        vm.ConnectParamsList[CurrentIndex()].PrinterPort = textBox.Text;
                        break;
                    case "TextBoxCamIP":
                        vm.ConnectParamsList[CurrentIndex()].CameraIP = textBox.Text;
                        break;
                    case "TextBoxControllerIP":
                        vm.ConnectParamsList[CurrentIndex()].ControllerIP = textBox.Text;
                        break;
                    case "TextBoxControllerPort":
                        vm.ConnectParamsList[CurrentIndex()].ControllerPort = textBox.Text;
                        break;
                    case "NumDelaySensor":
                        vm.ConnectParamsList[CurrentIndex()].DelaySensor = int.Parse(textBox.Text);
                        break;
                    case "NumDisSensor":
                        vm.ConnectParamsList[CurrentIndex()].DisableSensor = int.Parse(textBox.Text);
                        break;
                    case "NumPulseEncoder":
                        vm.ConnectParamsList[CurrentIndex()].PulseEncoder = int.Parse(textBox.Text);
                        break;
                    case "NumEncoderDia":
                        vm.ConnectParamsList[CurrentIndex()].EncoderDiameter = double.Parse(textBox.Text);
                        break;
                    case "TextBoxErrorField":
                        vm.ConnectParamsList[CurrentIndex()].FailedDataSentToPrinter = textBox.Text;
                        break;
                    default:
                        break;
                }
                vm.AutoSaveConnectionSetting(CurrentIndex());

            }), DispatcherPriority.Background);
        }
        private void TextBoxNormalParamsHandler(TextBox textBox)
        {
            textBox?.Dispatcher.BeginInvoke(new Action(() => // Use BeginInvoke to Update Input Last Value 
            {
                var vm = CurrentViewModel<MainViewModel>();
                if (vm == null) return;
                if (vm.ConnectParamsList == null) return;
                textBox.Text ??= string.Empty;
                switch (textBox.Name)
                {
                    case "TextBoxPrinterIP":
                        vm.ConnectParamsList[CurrentIndex()].PrinterIP = textBox.Text;
                        break;
                    case "TextBoxPrinterPort":
                        vm.ConnectParamsList[CurrentIndex()].PrinterPort = textBox.Text;
                        break;
                    case "TextBoxCamIP":
                        vm.ConnectParamsList[CurrentIndex()].CameraIP = textBox.Text;
                        break;
                    case "TextBoxControllerIP":
                        vm.ConnectParamsList[CurrentIndex()].ControllerIP = textBox.Text;
                        break;
                    case "TextBoxControllerPort":
                        vm.ConnectParamsList[CurrentIndex()].ControllerPort = textBox.Text;
                        break;
                    case "NumDelaySensor":
                        vm.ConnectParamsList[CurrentIndex()].DelaySensor = int.Parse(textBox.Text);
                        break;
                    case "NumDisSensor":
                        vm.ConnectParamsList[CurrentIndex()].DisableSensor = int.Parse(textBox.Text);
                        break;
                    case "NumPulseEncoder":
                        vm.ConnectParamsList[CurrentIndex()].PulseEncoder = int.Parse(textBox.Text);
                        break;
                    case "NumEncoderDia":
                        vm.ConnectParamsList[CurrentIndex()].EncoderDiameter = double.Parse(textBox.Text);
                        break;
                    case "TextBoxErrorField":
                        vm.ConnectParamsList[CurrentIndex()].FailedDataSentToPrinter = textBox.Text;
                        break;
                    default:
                        break;
                }
                vm.AutoSaveConnectionSetting(CurrentIndex());

            }), DispatcherPriority.Background);
        }
        private void ButtonWebView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int index = ListBoxMenu.SelectedIndex;
                var domain = CurrentViewModel<MainViewModel>()?.ConnectParamsList[index].PrinterIP;
                PrinterWebView pww = new()
                {
                    TitleContext = $"Printer {index + 1} Web Remote - Address: {domain}",
                    Address = domain
                };
                pww.Show();
            }
            catch (Exception) { }
        }





        private void Integer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (IsInitializing) return;
            try
            {
                var integerUpDown = sender as TAlex.WPF.Controls.NumericUpDown;
                if (integerUpDown?.ContentText == null) return;
                integerUpDown?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var vm = CurrentViewModel<MainViewModel>();
                    if (vm == null) return;
                  
                    switch (integerUpDown.Name)
                    {
                        case "NumDelaySensor":
                            vm.ConnectParamsList[CurrentIndex()].DelaySensor = int.Parse(integerUpDown.ContentText);
                            break;
                        case "NumDisSensor":
                            vm.ConnectParamsList[CurrentIndex()].DisableSensor = int.Parse(integerUpDown.ContentText);
                            break;
                        case "NumPulseEncoder":
                            vm.ConnectParamsList[CurrentIndex()].PulseEncoder = int.Parse(integerUpDown.ContentText);
                            break;
                        default:
                            break;
                    }
                    Debug.WriteLine("Vao day 1");
                    vm.AutoSaveConnectionSetting(CurrentIndex());

                }), DispatcherPriority.Background);
            }
            catch (Exception)
            {

            }
        }

        private void Double_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (IsInitializing) return;
            try
            {
                var doubleUpDown = sender as TAlex.WPF.Controls.NumericUpDown;
                if (doubleUpDown?.ContentText == null) return;
                doubleUpDown?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var vm = CurrentViewModel<MainViewModel>();
                    if (vm == null) return;
        
                    switch (doubleUpDown.Name)
                    {
                        case "NumEncoderDia":
                            vm.ConnectParamsList[CurrentIndex()].EncoderDiameter = double.Parse(doubleUpDown.ContentText);
                            break;

                        default:
                            break;
                    }
                    vm.AutoSaveConnectionSetting(CurrentIndex());
                }), DispatcherPriority.Background);
            }
            catch (Exception) { }
        }

        private void ToggleButtonState(object sender, RoutedEventArgs e)
        {
            if (IsInitializing) return;
            try
            {
                var toggleButton = sender as ToggleButton;
                toggleButton?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var vm = CurrentViewModel<MainViewModel>();
                    if (vm == null) return;
                    vm.ConnectParamsList[CurrentIndex()].EnController = (bool)toggleButton.IsChecked;
                    Debug.WriteLine("Vao day 3");
                    vm.AutoSaveConnectionSetting(CurrentIndex());
                }), DispatcherPriority.Background);
            }
            catch (Exception) { }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            CurrentViewModel<MainViewModel>()?.ConnectParamsList[CurrentIndex()].ResponseMessList.Clear();
        }

        private void ModeSendCheckedChange(object sender, RoutedEventArgs e)
        {
            if (IsInitializing) return;
            var rad = sender as RadioButton;
            try
            {
                rad?.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var vm = CurrentViewModel<MainViewModel>();
                    if (vm == null) return;
                   
                    if (rad.Name == "RadBasic" && rad.IsChecked == true)
                    {
                       vm.ConnectParamsList[CurrentIndex()].VerifyAndPrintBasicSentMethod = true;
                    }
                    else
                    {
                        vm.ConnectParamsList[CurrentIndex()].VerifyAndPrintBasicSentMethod = false;
                    }
                    vm.AutoSaveConnectionSetting(CurrentIndex());
                }), DispatcherPriority.Background);
            }
            catch (Exception){}
        }
        

        private void SelectPrintField(object sender, RoutedEventArgs e) // Select fields for verify and print send
        {
            var vm = CurrentViewModel<MainViewModel>();
            if (vm is null) return;
            VerifyAndPrintPODFormat verifyAndPrintPODFormat = new()
            {
                Index = CurrentIndex(),
                DataContext = vm
            };

            var res = verifyAndPrintPODFormat.ShowDialog(); // Show diaglog select POD
            if(res == true)
            {
                List<PODModel> _PODFormat = verifyAndPrintPODFormat._PODFormat;
                vm.ConnectParamsList[CurrentIndex()].PrintFieldForVerifyAndPrint = _PODFormat;
                vm.ConnectParamsList[CurrentIndex()].FormatedPOD = verifyAndPrintPODFormat.FormatedPOD;
                vm.AutoSaveConnectionSetting(CurrentIndex());
            }
        }

        private void DefaultClick(object sender, RoutedEventArgs e)
        {
            var vm = CurrentViewModel<MainViewModel>();
            if(vm is null) return;
            vm.ConnectParamsList[CurrentIndex()].FailedDataSentToPrinter = "Failure";    

        }
    }
}
