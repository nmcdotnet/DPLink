using DipesLink.ViewModels;
using SharedProgram.Models;
using System.Windows;
using System.Windows.Controls;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for VerifyAndPrintPODFormat.xaml
    /// </summary>
    public partial class VerifyAndPrintPODFormat : Window
    {
        public int Index { get; set; }
        public VerifyAndPrintPODFormat()
        {
            InitializeComponent();
            Loaded += VerifyAndPrintPODFormat_Loaded;
        }

        private void VerifyAndPrintPODFormat_Loaded(object sender, RoutedEventArgs e)
        {
            InitData();
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
        public List<PODModel> _PODFormat { get; private set; } = new();
        public string FormatedPOD { get; private set; }  = string.Empty;
        private void InitData()
        {
            for (int index = 1; index <= 20; index++) // Max 20 Field for Printer
            { 
                var podVCD = new PODModel(index, "", PODModel.TypePOD.FIELD, "");
                _PODFormat.Add(podVCD);
                listBoxPODLeft.Items.Add(podVCD);
            }
        }
        private void ListViewField_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewPodCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListViewPodCustom_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void AddingField()
        {
            try
            {
                var podTmp = ((PODModel)(listBoxPODLeft.SelectedItem));
                bool checkExist = false;
                foreach (object item in listBoxPODRight.Items)
                {
                    var podItem = (PODModel)(item);
                    if (podItem.Index == podTmp.Index)
                    {
                        checkExist = true;
                    }
                }

                if (!checkExist)
                {
                    if (listBoxPODRight.Items.Count == 0)
                    {
                        listBoxPODRight.Items.Add(podTmp.Clone());
                    }
                    else
                    {
                        for (int i = 0; i < listBoxPODRight.Items.Count; i++)
                        {
                            object item = listBoxPODRight.Items[i];
                            var podItem = (PODModel)(item);

                            if (podItem.Index == podTmp.Index + 1)
                            {
                                listBoxPODRight.Items.Insert(i, podTmp.Clone());
                                return;
                            }
                        }

                        object lastItem = listBoxPODRight.Items[listBoxPODRight.Items.Count - 1];
                        var lastPodItem = (PODModel)(lastItem);
                        if (lastPodItem.Index > podTmp.Index)
                        {
                            listBoxPODRight.Items.Insert(0, podTmp.Clone());
                        }
                        else if (lastPodItem.Index < podTmp.Index)
                        {
                            listBoxPODRight.Items.Add(podTmp.Clone());
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void Sample()
        {
            txtPrintFields.Text = "";
            foreach (object item in listBoxPODRight.Items)
            {
                var podTmp = (PODModel)item;
                txtPrintFields.Text += podTmp.ToString();
            }
        }

        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (listBoxPODLeft.SelectedItem == null)
            {
                return;
            }
            switch (btn.Name)
            {
                case "ButtonAdd":
                    AddingField();
                    Sample();
                    break;
                case "ButtonRemove":
                    if (listBoxPODRight.SelectedItem == null)
                    {
                        return;
                    }
                    listBoxPODRight.Items.Remove(listBoxPODRight.SelectedItem);
                    Sample();
                    break;
                case "ButtonClearAll":
                    listBoxPODRight.Items.Clear();
                    txtPrintFields.Text = "";
                    Sample();
                    break;
                default:
                    break;
            }
        }

        private void SavePODFormat(object sender, RoutedEventArgs e)
        {
            _PODFormat.Clear();
            foreach (object item in listBoxPODRight.Items)
            {
                var podTmp = (PODModel)item;
                if (podTmp.Type == PODModel.TypePOD.TEXT)
                {
                    podTmp.Value = txtPrintFields.Text;
                }
                _PODFormat.Add(podTmp);
                FormatedPOD = txtPrintFields.Text;
            }
            DialogResult = true;
        }

        private void ListViewPodCustom_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void listBoxPODLeft_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AddingField();
            Sample();
        }
    }
}
