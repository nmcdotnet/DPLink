using DipesLink.Models;
using DipesLink.ViewModels;
using DipesLink.Views.Converter;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DipesLink.Views.Extension
{
    public class CheckedObserHelper : ViewModelBase
    {
        #region Declarations    

        public int TotalChecked { get; set; }

        public int TotalPassed { get; set; }

        public int TotalFailed { get; set; }

        public ObservableCollection<CheckedResultModel>? CheckedList { get; set; } = new();

        private ObservableCollection<CheckedResultModel>? _DisplayList = new();
        public ObservableCollection<CheckedResultModel>? DisplayList
        {
            get { return _DisplayList; }
            set
            {
                if (_DisplayList != value)
                {
                    _DisplayList = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion End Declarations 

        #region Functionals
        private void GetStatistic()
        {
            if (CheckedList == null) return;
            //Get Number Checked
            TotalChecked = CheckedList.Count;
            TotalPassed = CheckedList.Count(x => x.Result == "Valid");
            if (TotalChecked >= TotalPassed)
            {
                TotalFailed = TotalChecked - TotalPassed;
            }
        }

        public void ConvertListToObservableCol(List<string[]> list)
        {
            CheckedList = new ObservableCollection<CheckedResultModel>(list.Select(data => new CheckedResultModel(data)));
            GetStatistic();
        }

        public DataTable GetDataTableDB()
        {
            string[] columnNames = new string[5] { "Index", "ResultData", "Result", "ProcessingTime", "DateTime" }; // Default header col
            var DataTable = new DataTable();
            var list = CheckedList?.ToList();
            if (list == null) return DataTable;
            try
            {
                // Add Header column
                int numberOfColumns = 5; // or get dynamic with list[0].Length;
                for (int i = 0; i < numberOfColumns; i++)
                {
                    string columnName = columnNames.Length > i ? columnNames[i] : $"Column{i + 1}";
                    DataTable.Columns.Add(columnName);
                }

                // Add row
                foreach (CheckedResultModel? array in list)
                {
                    DataRow row = DataTable.NewRow();
                    var properties = typeof(CheckedResultModel).GetProperties();

                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        object data = new();
                        switch (i)
                        {
                            case 0:
                                if (array.Index != null) data = array.Index;
                                break;
                            case 1:
                                if (array.ResultData != null) data = array.ResultData;
                                break;
                            case 2:
                                if (array.Result != null) data = array.Result;
                                break;
                            case 3:
                                if (array.ProcessingTime != null) data = array.ProcessingTime;
                                break;
                            case 4:
                                if (array.DateTime != null) data = array.DateTime;
                                break;
                            default:
                                break;
                        }
                        row[i] = data;
                    }
                    DataTable.Rows.Add(row);
                }
                return DataTable;
            }
            catch (Exception) { return DataTable; }
        }

        public void CreateDataTemplate(DataGrid dataGrid)
        {
            dataGrid.Columns.Clear();
            var properties = typeof(CheckedResultModel).GetProperties(); // get all properties of CheckedResultModel

            foreach (var property in properties)
            {
                if (property.Name == "Result") // Create template for "Result" column
                {
                    DataGridTemplateColumn templateColumn = new() { Header = property.Name, Width = DataGridLength.Auto };
                    DataTemplate template = new();
                    FrameworkElementFactory factory = new(typeof(Image));

                    Binding binding = new(property.Name)
                    {
                        Converter = new ResultCheckedImgConverter(),
                        Mode = BindingMode.OneWay
                    };

                    factory.SetValue(Image.SourceProperty, binding);
                    factory.SetValue(Image.HeightProperty, 20.0);
                    factory.SetValue(Image.WidthProperty, 20.0);

                    template.VisualTree = factory;
                    templateColumn.CellTemplate = template;
                    dataGrid.Columns.Add(templateColumn);
                }
                else
                {
                    DataGridTextColumn textColumn = new()
                    {
                        Header = property.Name,
                        Binding = new Binding(property.Name),
                        Width = DataGridLength.Auto
                    };
                    dataGrid.Columns.Add(textColumn);
                }
            }
        }

        public void AddNewData(string[] newData)
        {
            var data = new CheckedResultModel(newData);
            CheckedList?.Add(data);
            UpdateDisplayedData(data);
            GetStatistic();
        }

        public void TakeFirtloadCollection()
        {
            var newestItems = CheckedList?.OrderByDescending(x => x.DateTime).Take(100).ToList();
            if (DisplayList != null && newestItems != null)
            {
                DisplayList.Clear();
                foreach (var item in newestItems)
                {
                    DisplayList.Add(item);
                }
            }
        }

        public void UpdateDisplayedData(CheckedResultModel data)
        {
            if (CheckedList != null)
            {
                DisplayList?.Insert(0, data);
                while (DisplayList?.Count > 100)
                {
                    DisplayList.RemoveAt(DisplayList.Count - 1);
                }
            }
        }
        #endregion
    }
}
