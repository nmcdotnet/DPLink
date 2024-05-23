using DipesLink.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for CompareFormatWindow.xaml
    /// </summary>
    public partial class CompareFormatWindow : Window
    {
        public CompareFormatWindow()
        {
            InitializeComponent();
            Loaded += CompareFormatWindow_Loaded;
        }

        private void CompareFormatWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CallbackCommand(vm => _ = vm.LoadCsvIntoDataGrid());
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

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            CallbackCommand(vm => vm.OperationPODFormat(bt.Name));
            CallbackCommand(vm => TextBoxResultFormatPOD.Text = vm.TextPODResult);
        }

        private void SavePODFormat(object sender, RoutedEventArgs e)
        {
            CallbackCommand(vm => vm.SavePODFormat());
            this.Close();
        }

        private void TextFieldInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;   
            CallbackCommand(vm => vm.TextInputChange(tb.Text));
            CallbackCommand(vm => TextBoxResultFormatPOD.Text = vm.TextPODResult);
        }

        private void ListViewPodCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            CallbackCommand(vm => vm.EnableTextFieldInput());
            CallbackCommand(vm => TextBoxFieldInput.Text = vm.TextFieldFeedback);
            CallbackCommand(vm => TextBoxResultFormatPOD.Text = vm.TextPODResult);
        }

        private void ListViewPodCustom_LostFocus(object sender, RoutedEventArgs e)
        {
          
        }

        private void ListViewField_GotFocus(object sender, RoutedEventArgs e)
        {
            ListViewPodCustom.SelectedItem = null;
            CallbackCommand(vm => vm.EnableTextFieldInput());
            CallbackCommand(vm => TextBoxFieldInput.Text = vm.TextFieldFeedback);
            CallbackCommand(vm => TextBoxResultFormatPOD.Text = vm.TextPODResult);
        }

        private void DBView_Click(object sender, RoutedEventArgs e)
        {
            CallbackCommand(vm => vm.DBView());
        }

        private void ListViewPodCustom_GotFocus(object sender, RoutedEventArgs e)
        {
            ListViewField.SelectedItem = null;
        }

        private void ButtonFormatView_Click(object sender, RoutedEventArgs e)
        {
            CallbackCommand(vm => vm.PreviewPODList());
        }
    }
}
