using DipesLink.Models;
using DipesLink.Views.Converter;
using DipesLink.Views.Extension;
using SharedProgram.Shared;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Cloudtoid;
using System.Windows.Input;

namespace DipesLink.Views.SubWindows
{
    /// <summary>
    /// Interaction logic for JobLogsWindow.xaml
    /// </summary>
    public partial class PrintedLogsWindow : Window
    {
        JobLogsDataTableHelper? _jobLogsDataTableHelper = new();
        public DataTable? PrintedDataTable { get; set; } = new();
        private string? _pageInfo;
        private JobOverview _currentJob;

        public PrintedLogsWindow(JobOverview currentJob)
        {
            _currentJob = currentJob;
            InitializeComponent();
            Loaded += JobLogsWindow_Loaded;
            Closed += PrintedLogsWindow_Closed;
        }

        private void PrintedLogsWindow_Closed(object? sender, EventArgs e)
        {
            if (PrintedDataTable != null)
            {
                PrintedDataTable.Clear();
                PrintedDataTable.Dispose();
                PrintedDataTable = null;
            }

            if (_jobLogsDataTableHelper != null)
            {
                _jobLogsDataTableHelper.Dispose();
                _jobLogsDataTableHelper = null;
            }

            _currentJob = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
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

        public void CreateDataTemplate(DataGrid dataGrid)
        {
            dataGrid.Columns.Clear();
            foreach (DataColumn column in PrintedDataTable.Columns)
            {
                if (column.ColumnName == "Status") // Create template for "Result" column
                {
                    DataGridTemplateColumn templateColumn = new() { Header = column.ColumnName, Width = DataGridLength.Auto };
                    DataTemplate template = new();
                    FrameworkElementFactory factory = new(typeof(Image));

                    Binding binding = new(column.ColumnName)
                    {
                        Converter = new StatusToIconConverter(),
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
                        Header = column.ColumnName,
                        Binding = new Binding(column.ColumnName),
                        Width = DataGridLength.Auto
                    };
                    dataGrid.Columns.Add(textColumn);
                }
            }
        }

        public static async Task<DataTable> InitDatabaseAsync(List<string[]> dbList)
        {
            var printedDataTable = new DataTable();
            if (dbList == null || dbList.IsEmpty())
            {
                return printedDataTable;
            }
            await Task.Run(() =>
            {
                foreach (var header in dbList[0]) // add column
                {
                    printedDataTable.Columns.Add(header);
                }
                for (int i = 1; i < dbList.Count; i++) // add Row data
                {
                    printedDataTable.Rows.Add(dbList[i]);
                }
                return printedDataTable;
            });
            return printedDataTable;
        }

        private void UpdateNumber()
        {
            try
            {
                TextBlockTotal.Text = PrintedDataTable?.Rows.Count.ToString();
                TextBlockPrinted.Text = _currentJob.PrintedDataNumber;
                if(int.TryParse(TextBlockTotal.Text, out int total) && int.TryParse(TextBlockPrinted.Text, out int printed))
                {
                    TextBlockWait.Text = (total - printed).ToString();
                }
              
            }
            catch (Exception)
            {

            }
           
        }
        private async void JobLogsWindow_Loaded(object sender, RoutedEventArgs e)
        {
          
            _jobLogsDataTableHelper = new JobLogsDataTableHelper();

            var taskLoadPrintedList = InitDatabaseAndPrintedStatusAsync();
            await taskLoadPrintedList;
            var listData = taskLoadPrintedList.Result;

            CreateDataTemplate(DataGridResult);
            var taskCreateDataTable = InitDatabaseAsync(listData);
            await taskCreateDataTable;
            PrintedDataTable = taskCreateDataTable.Result;

            if (_jobLogsDataTableHelper == null) return;
            if (PrintedDataTable != null)
            {
                _jobLogsDataTableHelper.InitDatabase(PrintedDataTable, DataGridResult);
            }
            UpdateNumber(); 
            UpdatePageInfo();
        }

        private void UpdatePageInfo()
        {
            _pageInfo = string.Format("Page {0} of {1} ({2} items)", _jobLogsDataTableHelper?.Paginator?.CurrentPage + 1, _jobLogsDataTableHelper?.Paginator?.TotalPages, _jobLogsDataTableHelper?.NumberItemInCurPage);
            TextBlockPageInfo.Text = _pageInfo;
            TextBoxPage.Text = (_jobLogsDataTableHelper?.Paginator?.CurrentPage + 1).ToString(); // Get current page for Textbox Page
        }

        private void PageAction_Click(object sender, RoutedEventArgs e)
        {
            if (_jobLogsDataTableHelper == null || _jobLogsDataTableHelper.Paginator == null) return;
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "ButtonFirst":
                    _jobLogsDataTableHelper.UpdateDataGrid(DataGridResult, 1);
                    break;

                case "ButtonBack":
                    if (_jobLogsDataTableHelper.Paginator.PreviousPage())
                    {
                        _jobLogsDataTableHelper.UpdateDataGrid(DataGridResult);
                    }
                    break;

                case "ButtonNext":
                    if (_jobLogsDataTableHelper.Paginator.NextPage())
                    {
                        _jobLogsDataTableHelper.UpdateDataGrid(DataGridResult);
                    }
                    break;

                case "ButtonEnd":
                    _jobLogsDataTableHelper.UpdateDataGrid(DataGridResult, _jobLogsDataTableHelper.Paginator.TotalPages);
                    break;

                case "ButtonGotoPage":
                    GotoPageAction();
                    break;
                default:
                    break;
            }
            ButtonPaginationVis();
            UpdatePageInfo();
        }

        private void ButtonPaginationVis()
        {
            if (_jobLogsDataTableHelper == null || _jobLogsDataTableHelper.Paginator == null) return;

            //Button Next - End
            if (_jobLogsDataTableHelper.Paginator.CurrentPage + 1 == _jobLogsDataTableHelper.Paginator.TotalPages)
            {
                ButtonNext.IsEnabled = false;
                ButtonEnd.IsEnabled = false;
            }
            else
            {
                ButtonNext.IsEnabled = true;
                ButtonEnd.IsEnabled = true;
            }

            // Button Back - First
            if (_jobLogsDataTableHelper.Paginator.CurrentPage + 1 == 1)
            {
                ButtonBack.IsEnabled = false;
                ButtonFirst.IsEnabled = false;
            }
            else
            {
                ButtonBack.IsEnabled = true;
                ButtonFirst.IsEnabled = true;
            }
        }

        private void GotoPageAction()
        {
            if (_jobLogsDataTableHelper == null || _jobLogsDataTableHelper.Paginator == null) return;
            if (int.TryParse(TextBoxPage.Text, out int page))
            {
                if (page > 0 && page <= _jobLogsDataTableHelper.Paginator.TotalPages)
                {
                    _jobLogsDataTableHelper.UpdateDataGrid(DataGridResult, page);
                }
                else
                {
                    MessageBox.Show("Page not found", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Page not found", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_jobLogsDataTableHelper == null || _jobLogsDataTableHelper.Paginator == null) return;
            ComboBox cbb = (ComboBox)sender;
            JobLogsDataTableHelper.FilteredKeyword type = JobLogsDataTableHelper.FilteredKeyword.All;
            switch (cbb.SelectedIndex)
            {
                case 0: 
                    type = JobLogsDataTableHelper.FilteredKeyword.All;
                    break;
                case 1: 
                    type = JobLogsDataTableHelper.FilteredKeyword.Waiting;
                    break;
                case 2: 
                    type = JobLogsDataTableHelper.FilteredKeyword.Printed;
                    break;
                default: break;
            }

           // Tác vụ search nặng không hoạt động
            await _jobLogsDataTableHelper.DatabaseFilteredForPrintStatusAsync(DataGridResult, type);
           
            UpdatePageInfo();
            ButtonPaginationVis();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchAction(TextBoxSearch.Text);
        }

        private void ButtonRF_Click(object sender, RoutedEventArgs e)
        {
            TextBoxSearch.Text = "";
            SearchAction("");
        }

        private void SearchAction(string keyword)
        {
            if (_jobLogsDataTableHelper == null || _jobLogsDataTableHelper.Paginator == null) return;
            _jobLogsDataTableHelper.DatabaseSearchForPrintStatus(DataGridResult, keyword);
            UpdatePageInfo();
            ButtonPaginationVis();
        }

        private void DataGridResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        #region Get Cell Value
       
        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow row, int column)
        {
            if (row != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(row);

                if (presenter == null)
                {
                    dataGrid.ScrollIntoView(row, dataGrid.Columns[column]);
                    presenter = FindVisualChild<DataGridCellsPresenter>(row);
                }

                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                return cell;
            }
            return null;
        }
        public static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        #endregion

        private void DataGridResult_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Close all detail
            foreach (var item in DataGridResult.Items)
            {
                DataGridRow row = (DataGridRow)DataGridResult.ItemContainerGenerator.ContainerFromItem(item);
                if (row != null) row.DetailsVisibility = Visibility.Collapsed;
            }
        }

        private void ButtonRePrint_Click(object sender, RoutedEventArgs e)
        {
            var currentJob = CurrentViewModel<JobOverview>();
            if (currentJob != null)
            {
                currentJob.RaiseReprint(currentJob.Index);
            }
        }

        private async Task<List<string[]>> InitDatabaseAndPrintedStatusAsync()
        {
            var path = SharedPaths.PathSubJobsApp + $"{_currentJob.Index + 1}\\" + "printedPathString";

            var pathstr = SharedFunctions.ReadStringOfPrintedResponePath(path);
            // Get path db and printed list
            var pathDatabase = _currentJob?.DatabasePath;
            var pathBackupPrintedResponse = SharedPaths.PathPrintedResponse + $"Job{_currentJob?.Index + 1}\\" + pathstr;

            // Init Databse from file, add index column, status column, and string "Feild"
            List<string[]> tmp = await Task.Run(() => { return InitDatabase(pathDatabase); });

            // Update Printed status
            if (pathstr != "" && File.Exists(_currentJob?.DatabasePath) && tmp.Count > 1)
            {
                await Task.Run(() => { InitPrintedStatus(pathBackupPrintedResponse, tmp); });
            }
            return tmp;
        }

        private static List<string[]> InitDatabase(string? path)
        {
            List<string[]> result = new(); // List result
            if (!File.Exists(path))
            {
                return result;
            }
            try
            {

                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); // Only Read,
                using var reader = new StreamReader(fileStream, Encoding.UTF8, true);
                var rexCsvSplitter = path.EndsWith(".csv") ? new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))") : new Regex(@"[\t]");
                int lineCounter = -1;
                int columnCount = 0;
                while (!reader.EndOfStream)
                {
                    string[] line = rexCsvSplitter.Split(reader.ReadLine()).Select(x => Csv.Unescape(x)).ToArray();
                    lineCounter++;
                    if (lineCounter == 0)
                    {
                        // Create additional database columns
                        var tmp = new string[line.Length + 2];
                        tmp[0] = "Index"; // thêm cột Index ở đầu
                        tmp[^1] = "Status"; // Thêm cột status ở cuối (^1 đại diện phần tử cuối)
                        for (int i = 1; i < tmp.Length - 1; i++)
                        {
                            tmp[i] = line[i - 1] + $" - Field{i}"; // Thêm chữ field vào cạnh column
                        }
                        columnCount = tmp.Length;
                        result.Add(tmp);
                    }
                    else
                    {
                        // ignore empty line 
                        //if (line.Length == 1 && line[0] == "") continue;
                        // handle database row before adding
                        var tmp = new string[columnCount];
                        tmp[0] = "" + lineCounter;
                        tmp[columnCount - 1] = "Waiting"; // thêm trạng thái waiting cho trường status
                        for (int i = 1; i < tmp.Length - 1; i++)
                        {
                            if (i - 1 < line.Length)
                            {
                                tmp[i] = line[i - 1];
                            }
                            else
                            {
                                tmp[i] = "";
                            }
                        }
                        result.Add(tmp);
                    }
                }
            }
            catch (IOException) { }
            catch (Exception) { }
            return result;
        }

        private static void InitPrintedStatus(string pathBackupPrinted, List<string[]> dbList)
        {
            if (!File.Exists(pathBackupPrinted))
            {
                return;
            }
            try
            {
                using StreamReader reader = new(pathBackupPrinted, Encoding.UTF8, true);
                int i = -1;
                var rexCsvSplitter = pathBackupPrinted.EndsWith(".csv") ? new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))") : new Regex(@"[\t]");
                while (!reader.EndOfStream)
                {
                    i++;
                    if (i == 0) // Leave out the first row
                    {
                        reader.ReadLine();
                    }
                    else
                    {
                        string line = Csv.Unescape(rexCsvSplitter.Split(reader.ReadLine())[0]);
                        if (int.TryParse(line, out int index))
                        {
                            dbList[index][^1] = "Printed"; // Get rows by index and last column ^1 Updated with the content Printed
                        }
                    }
                }
            }
            catch (IOException) { }
            catch (Exception) { }
        }

        private void TextBoxSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonSearch_Click(sender, e);
            }

        }
    }
}
