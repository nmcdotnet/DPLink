using DipesLink.ViewModels;
using DipesLink.Views.Extension;
using SharedProgram.Shared;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DipesLink.Views.UserControls.MainUc
{
    /// <summary>
    /// Interaction logic for JobEventsLog.xaml
    /// </summary>
    public partial class JobEventsLog : UserControl
    {
        JobEventsLogHelper _jobEventsLogHelper = new();
        public JobEventsLog()
        {
            InitializeComponent();
            this.Loaded += JobEventsLog_Loaded;
           ViewModelSharedEvents.OnListBoxMenuSelectionChange += MainWindow_ListBoxMenuSelectionChange;
        }

        private void MainWindow_ListBoxMenuSelectionChange(object? sender, EventArgs e)
        {
            var index = (int)sender;
            if (index == 3) // Is Event Log
            {
                LoadEventDataFromFile();
            }
        }

        private void JobEventsLog_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

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

        private void ListBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadEventDataFromFile();
        }


        private void LoadEventDataFromFile()
        {
            Application.Current?.Dispatcher.Invoke(new Action(() =>
                {
                    int index = ListBoxMenu.SelectedIndex;
                    _jobEventsLogHelper?.DisplayList?.Clear();
                    DataGridEventLogs.AutoGenerateColumns = false;
                    _jobEventsLogHelper?.CreateDataTemplate(DataGridEventLogs);
                    var logPath = CurrentViewModel<MainViewModel>()?.JobList[index].EventsLogPath;
                    var logDirectoryPath = SharedPaths.PathEventsLog + $"Job{index + 1}";
                    logPath = $"{logDirectoryPath}\\_JobEvents_{CurrentViewModel<MainViewModel>()?.JobList[index].Name}.csv";
                   
                    if (logPath != null && File.Exists(logPath))
                    {
                        _jobEventsLogHelper?.InitEventsLogDatabase(logPath);
                    }
                    DataGridEventLogs.ItemsSource = _jobEventsLogHelper?.DisplayList;
                }));
        }
    }
}
