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

namespace DipesLink.Views.UserControls.CustomControl
{
    /// <summary>
    /// Interaction logic for IpAddressControl.xaml
    /// </summary>
    public partial class IpAddressControl : UserControl
    {
        // Định nghĩa RoutedEvent
        //public static readonly RoutedEvent TextChangedEvent = EventManager.RegisterRoutedEvent(
        //    "TextChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(IpAddressControl));

        //// .NET wrapper
        //public event RoutedEventHandler TextChanged
        //{
        //    add { AddHandler(TextChangedEvent, value); }
        //    remove { RemoveHandler(TextChangedEvent, value); }
        //}
        // Định nghĩa sự kiện TextChanged tùy chỉnh
        public event TextChangedEventHandler TextChanged;

        public IpAddressControl()
        {
            InitializeComponent();
            TextBoxPart1.PreviewTextInput += IpPart_PreviewTextInput;
            TextBoxPart1.PreviewKeyDown += IpPart_PreviewKeyDown;
            TextBoxPart2.PreviewTextInput += IpPart_PreviewTextInput;
            TextBoxPart2.PreviewKeyDown += IpPart_PreviewKeyDown;
            TextBoxPart3.PreviewTextInput += IpPart_PreviewTextInput;
            TextBoxPart3.PreviewKeyDown += IpPart_PreviewKeyDown;
            TextBoxPart4.PreviewTextInput += IpPart_PreviewTextInput;
            TextBoxPart4.PreviewKeyDown += IpPart_PreviewKeyDown;

            TextBoxPart1.TextChanged += UpdateText;
            TextBoxPart2.TextChanged += UpdateText;
            TextBoxPart3.TextChanged += UpdateText;
            TextBoxPart4.TextChanged += UpdateText;
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
   "Text", typeof(string), typeof(IpAddressControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextChanged));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IpAddressControl)d;
            string[] parts = ((string)e.NewValue)?.Split('.') ?? new string[0];
            if (parts.Length == 4)
            {
                control.TextBoxPart1.Text = parts[0];
                control.TextBoxPart2.Text = parts[1];
                control.TextBoxPart3.Text = parts[2];
                control.TextBoxPart4.Text = parts[3];
            }
        }
        private void UpdateText(object sender, TextChangedEventArgs e)
        {
            Text = $"{TextBoxPart1.Text}.{TextBoxPart2.Text}.{TextBoxPart3.Text}.{TextBoxPart4.Text}";
            TextChanged?.Invoke(this, e);
        }
        private void IpPart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !int.TryParse(e.Text, out int value);
            var tb = sender as TextBox;
            if (tb != null && tb.Text.Length >= 3)
            {
                e.Handled = true;
            }

        }
        private void IpPart_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if ((e.Key == Key.OemPeriod || e.Key == Key.Decimal) && tb.Text.Length == 3)
                {
                    e.Handled = true;
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                }
                else if (e.Key == Key.Space || e.Key == Key.Enter && tb.Text.Length == 3)
                {
                    e.Handled = true;
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        tb.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }), System.Windows.Threading.DispatcherPriority.ContextIdle);
                }
            }
        }
    }
}
