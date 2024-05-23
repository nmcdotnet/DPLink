using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DipesLink.Views.UserControls.CustomControl
{
    public class FloatUpDown : Control
    {
        private TextBox _textBox;
        private Button _upButton;
        private Button _downButton;

        static FloatUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatUpDown), new FrameworkPropertyMetadata(typeof(FloatUpDown)));
        }

        public enum NumberType
        {
            Integer,
            Float
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(FloatUpDown), new PropertyMetadata(0.0));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(FloatUpDown), new PropertyMetadata(1.0));

        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(NumberType), typeof(FloatUpDown), new PropertyMetadata(NumberType.Float));

        public NumberType Type
        {
            get { return (NumberType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBox = GetTemplateChild("PART_TextBox") as TextBox;
            _upButton = GetTemplateChild("PART_UpButton") as Button;
            _downButton = GetTemplateChild("PART_DownButton") as Button;

            if (_textBox != null)
            {
                _textBox.Text = Value.ToString();
                _textBox.TextChanged += OnTextChanged;
                _textBox.PreviewTextInput += OnPreviewTextInput;
                DataObject.AddPastingHandler(_textBox, OnPaste);
            }

            if (_upButton != null)
            {
                _upButton.Click += (s, e) => { Value += Increment; UpdateUI(); };
            }

            if (_downButton != null)
            {
                _downButton.Click += (s, e) => { Value -= Increment; UpdateUI(); };
            }
        }
        private void UpdateUI()
        {
            if (Type == NumberType.Integer)
                _textBox.Text = Value.ToString();
            else
                _textBox.Text = Value.ToString("F2");
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            if (Type == NumberType.Integer)
            {
                if (int.TryParse(_textBox.Text, out int intResult))
                {
                    result = intResult;
                }
                else
                {
                    result = 0;
                }
            }
            else
            {
                if (double.TryParse(_textBox.Text, out double doubleResult))
                {
                    result = doubleResult;
                }
                else
                {
                    result = 0.0;
                }
            }

            Value = result;
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextAllowed(string text)
        {
            foreach (char c in text)
            {
                if (!Char.IsDigit(c) && c != '.' && c != '-')
                {
                    return false;
                }

                if (Type == NumberType.Integer && c == '.')
                {
                    return false;
                }
            }
            return true;
        }
    }
    //public class FloatUpDown : Control
    //{
    //    private TextBox _textBox;
    //    private Button _upButton;
    //    private Button _downButton;

    //    static FloatUpDown()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatUpDown), new FrameworkPropertyMetadata(typeof(FloatUpDown)));
    //    }

    //    public static readonly DependencyProperty ValueProperty =
    //        DependencyProperty.Register("Value", typeof(float), typeof(FloatUpDown), new PropertyMetadata(0f));

    //    public float Value
    //    {
    //        get { return (float)GetValue(ValueProperty); }
    //        set { SetValue(ValueProperty, value); }
    //    }

    //    public static readonly DependencyProperty IncrementProperty =
    //        DependencyProperty.Register("Increment", typeof(float), typeof(FloatUpDown), new PropertyMetadata(0.1f));

    //    public float Increment
    //    {
    //        get { return (float)GetValue(IncrementProperty); }
    //        set { SetValue(IncrementProperty, value); }
    //    }

    //    public override void OnApplyTemplate()
    //    {
    //        base.OnApplyTemplate();

    //        _textBox = GetTemplateChild("PART_TextBox") as TextBox;
    //        _upButton = GetTemplateChild("PART_UpButton") as Button;
    //        _downButton = GetTemplateChild("PART_DownButton") as Button;

    //        if (_textBox != null)
    //        {
    //            _textBox.Text = Value.ToString();
    //            _textBox.TextChanged += OnTextChanged;
    //            _textBox.PreviewTextInput += OnPreviewTextInput;
    //            DataObject.AddPastingHandler(_textBox, OnPaste);
    //        }

    //        if (_upButton != null)
    //        {
    //            _upButton.Click += (s, e) => 
    //            {
    //                Value += Increment;
    //                _textBox.Text = Value.ToString("F2");
    //            };
    //        }

    //        if (_downButton != null)
    //        {
    //            _downButton.Click += (s, e) => 
    //            { 
    //                Value -= Increment;
    //                _textBox.Text = Value.ToString("F2"); 
    //            };
    //        }
    //    }

    //    private void OnTextChanged(object sender, TextChangedEventArgs e)
    //    {
    //        if (float.TryParse(_textBox.Text, out float result))
    //        {
    //            Value = result;
    //        }
    //    }

    //    private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    //    {
    //        e.Handled = !IsTextAllowed(e.Text);
    //    }

    //    private void OnPaste(object sender, DataObjectPastingEventArgs e)
    //    {
    //        if (e.DataObject.GetDataPresent(typeof(string)))
    //        {
    //            string text = (string)e.DataObject.GetData(typeof(string));
    //            if (!IsTextAllowed(text))
    //            {
    //                e.CancelCommand();
    //            }
    //        }
    //        else
    //        {
    //            e.CancelCommand();
    //        }
    //    }

    //    private bool IsTextAllowed(string text)
    //    {
    //        foreach (char c in text)
    //        {
    //            if (!Char.IsDigit(c) && c != '.' && c != '-')
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }
    //}
}
