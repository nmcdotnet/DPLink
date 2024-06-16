using DipesLink.Models;
using DipesLink.Views.Converter;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace DipesLink.Views.Extension
{
    public class PrintingDataTableHelper
    {
        public Paginator? Paginator { get; set; }
        public JobOverview? CurrentViewModel { get; private set; }
        private List<string[]>? _orgDBList;
        public int PrintedNumber { get; set; }
        private int _rowIndex;
        public event EventHandler? OnDetectMissPrintedCode;
        public DataTable? PrintedDataTable { get; private set; }
        public void RaiseDetectMissPrintedCode()
        {
            OnDetectMissPrintedCode?.Invoke(CurrentViewModel?.Index, EventArgs.Empty);
        }

        /// <summary>
        /// Load DB to DataGrid and Limit Row
        /// </summary>
        /// <param name="dbList"></param>
        /// <param name="dataGrid"></param>
        /// <param name="currentPage"></param>
        /// <param name="currentViewModel"></param>
        public async Task InitDatabaseAsync(List<string[]> dbList, DataGrid dataGrid, int currentPage, JobOverview? currentViewModel)
        {
            PrintedDataTable = new();
            //DataTable dataTable = new();
            await Task.Run(() =>
            {
                foreach (var header in dbList[0]) // add column
                {
                    PrintedDataTable.Columns.Add(header);
                }
                for (int i = 1; i < dbList.Count; i++) // add Row data
                {
                    PrintedDataTable.Rows.Add(dbList[i]);
                }
                _orgDBList = dbList;
                CounterPrintedFirstLoad(PrintedDataTable);
               
            });

            // Update UI after datatable loaded
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (currentViewModel == null) return;
                CurrentViewModel = currentViewModel;
                dataGrid.Columns.Clear();
                ProcessMiniPage(dataGrid, PrintedDataTable, currentPage);
                currentViewModel.IsShowLoadingDB = Visibility.Collapsed;
            });
        }
      
        private int FindIndexByData(string[] printedCode, List<string[]> dbList) => dbList
            .FindIndex(x => x.Take(x.Length - 1).SequenceEqual(printedCode.Take(printedCode.Length - 1))) - 1;

        private void CounterPrintedFirstLoad(DataTable dataTable)
        {
            PrintedNumber = dataTable.Select("Status = 'Printed'").Length;
        }

        /// <summary>
        /// Status change 
        /// </summary>
        /// <param name="printedCode"></param>
        /// <param name="currentViewModel"></param>
        /// <param name="dataGrid"></param>
        public void ChangeStatusOnDataGrid(string[] printedCode, JobOverview? currentViewModel, DataGrid dataGrid)
        {
            if (currentViewModel == null) return;
            string rowIdentifier = printedCode[0];
            string newStatus = printedCode[^1];
            CurrentViewModel = currentViewModel;
            int _rowIndexTotal = 0;
            if (_orgDBList != null)
            {
                _rowIndexTotal = FindIndexByData(printedCode, _orgDBList);
            }

            // Debug.WriteLine("Page: " + CurrentViewModel.CurrentPage);
            //  Debug.WriteLine("MyIndex: " + CurrentViewModel.CurrentIndex);
            int currentPage = Paginator.GetCurrentPageNumber(_rowIndexTotal);
            if (Paginator != null && currentPage != Paginator.CurrentPage)
            {
                Paginator.CurrentPage = currentPage;
                UpdateDataGrid(dataGrid);
            }
            foreach (DataRow row in CurrentViewModel.MiniDataTable.Rows)
            {
                _rowIndex = CurrentViewModel.MiniDataTable.Rows.IndexOf(row);

                if (row[0].ToString() == rowIdentifier)
                {
                    row["Status"] = newStatus;
                    if (newStatus != "Printed")
                    {
                        RaiseDetectMissPrintedCode();
                    }

                    break;
                }
            }
            ScrollIntoView(_rowIndex, dataGrid);
        }

        /// <summary>
        /// Creates a small page of 500 lines of data (default)
        /// </summary>
        /// <param name="dataGrid"></param>
        /// <param name="dataTable"></param>
        public void ProcessMiniPage(DataGrid dataGrid, DataTable dataTable, int currentPage)
        {
            Paginator = new Paginator(dataTable);
            if (Paginator == null) return;
            Paginator.CurrentPage = currentPage;
            foreach (DataColumn column in Paginator.GetPage(Paginator.CurrentPage).Columns)
            {
                if (column.ColumnName == "Status")
                {
                    DataGridTemplateColumn templateColumn = new() { Header = column.ColumnName, Width = DataGridLength.Auto };
                    DataTemplate template = new();
                    FrameworkElementFactory factory = new(typeof(Image)); // Create Image UI by Code behind instead XAML
                    Binding binding = new(column.ColumnName) { Converter = new StatusToIconConverter() };

                    factory.SetValue(Image.SourceProperty, binding); // Set binding for Image.
                    factory.SetValue(Image.HeightProperty, 20.0); // Image Height
                    factory.SetValue(Image.WidthProperty, 20.0);  // Image Width

                    template.VisualTree = factory; // add UI to VisualTree Template
                    templateColumn.CellTemplate = template; // CellTemplate = Template
                    dataGrid.Columns.Add(templateColumn); // Add DataGridTemplateColumn
                }
                else
                {
                    DataGridTextColumn textColumn = new()
                    {
                        Header = column.ColumnName,
                        Binding = new Binding(column.ColumnName),
                        Width = 100
                    };
                    dataGrid.Columns.Add(textColumn);
                }
            }
            UpdateDataGrid(dataGrid);
        }

        /// <summary>
        /// Update Mini DataGrid 
        /// </summary>
        /// <param name="dataGrid"></param>
        public void UpdateDataGrid(DataGrid dataGrid)
        {
            if (Paginator != null)
            {
                if (CurrentViewModel == null) return;
                CurrentViewModel.MiniDataTable = Paginator.GetPage(Paginator.CurrentPage); // Load mini datatable by current page
                dataGrid.AutoGenerateColumns = false;
                dataGrid.ItemsSource = CurrentViewModel.MiniDataTable.DefaultView;
            }
        }

        /// <summary>
        /// Scroll to row DataGrid
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="dataGrid"></param>
        public static void ScrollIntoView(int rowIndex, DataGrid dataGrid)
        {
            if (rowIndex >= 0 && rowIndex < dataGrid.Items.Count)
            {
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                dataGrid.SelectedIndex = rowIndex;
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
                row?.Focus();
            }
        }

    }
}
