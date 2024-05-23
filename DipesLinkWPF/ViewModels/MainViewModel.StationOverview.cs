using DipesLink.Models;
using DipesLink.Views.Converter;
using DipesLink.Views.Extension;
using SharedProgram.Models;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static DipesLink.Views.Enums.ViewEnums;
namespace DipesLink.ViewModels
{
    public partial class MainViewModel
    {
        //private JobOverview _currentStation = new();
        //public JobOverview CurrentStation
        //{
        //    get { return _currentStation; }
        //    set
        //    {
        //        if (_currentStation != value)
        //        {
        //            _currentStation = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        #region CommandProperties  
        public ICommand StartCommand { get; set;}
        public ICommand StopCommand { get; set;}
        public ICommand TriggerCommand { get; set;}
        public ICommand ViewCommand { get; set;}
        #endregion

        //public void InitStation()
        //{
        //    JobList = new();
        //    for (int i = 0; i < 4; i++)
        //    {
        //        JobOverview jobOverview = new()
        //        {
        //            Index = i,
        //            Name = "Job " + i
        //        };
        //        JobList.Add(jobOverview);
        //    }
           
        //}
        internal void CheckStationChange()
        {
            if (StationSelectedIndex + 1 != ViewModelSharedValues.Settings.NumberOfStation)
            {

                var res = CusMsgBox.Show("Change number of station !", "Change Station", ButtonStyleMessageBox.OKCancel, ImageStyleMessageBox.Warning);
                if (res)
                {

                    ViewModelSharedValues.Settings.NumberOfStation = StationSelectedIndex + 1;
                    ViewModelSharedFunctions.SaveSetting();

                    string exePath = Process.GetCurrentProcess().MainModule.FileName;
                    Process.Start(exePath);
                    Application.Current.Shutdown();

                }
                else
                {
                    StationSelectedIndex = ViewModelSharedValues.Settings.NumberOfStation - 1;
                }
            }
        }

        #region Button Control

        int count = 10;
        private void TriggerCommandAction(JobOverview? job)
        {

            if (count == 100) count = 0;
            count += 10;

            //JobList[0].ValueChart = new LiveChartsCore.Defaults.ObservableValue(90);
            //JobList[0].GaugeItem = new GaugeItem(JobList[0].ChartValue,
            //   series =>
            //   {
            //       series.MaxRadialColumnWidth = 10;
            //       series.DataLabelsSize = 30;
            //   });
        }

        private void StopCommandAction(JobOverview? job)
        {
            if (job == null) return;
            job.StatusStopButton = false;
            job.StatusStartButton = true;
            job.StatusColor = new SolidColorBrush(Colors.Red);
            job.StatusText = "STOP";
        }

        private void StartCommandAction(JobOverview? job)
        {
            if (job == null) return;
            job.StatusStartButton = false;
            job.StatusStopButton = true;
            job.StatusColor = new SolidColorBrush(Colors.Green);
            job.StatusText = "RUN";
        }

        private void ViewCommandAction(JobOverview? job)
        {
            if (job == null) return;
            JobIndex = job.Index;
          //  CurrentStation = JobList[job.Index];
            GetCurrentJobDetail(job.Index);
            JobViewVisibility = Visibility.Collapsed;
            JobViewVisibility1 = Visibility.Visible;
        }

        internal void UpdateJobInfo(int index)
        {
            GetCurrentJobDetail(index);
        }

        internal void ChangeJobByDeviceStatSymbol(int index)
        {
            JobIndex = index;
           // CurrentStation = JobList[index];
            GetCurrentJobDetail(index);
        }

        internal void MinusWidth()
        {
            if (ActualWidthMainWindow >= 1800)
            {
                ActualWidthGrid = 1800;
            }
        }

        /// <summary>
        /// Update Basic Params for Job
        /// </summary>
        /// <param name="index"></param>
       

        internal void JobCreationSelectionChanged(int jobIndex)
        {
           //CurrentJobSetting = JobSettingList[jobIndex];
        }

        #endregion Button Control
    }
}
