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

        internal void UpdateJobInfo(int index)
        {
            GetCurrentJobDetail(index);
        }

        internal void ChangeJobByDeviceStatSymbol(int index)
        {
            JobIndex = index;
            GetCurrentJobDetail(index);
        }

        internal void MinusWidth()
        {
            if (ActualWidthMainWindow >= 1800)
            {
                ActualWidthGrid = 1800;
            }
        }

        private string _TitleApp = "Stations Details";

        public string TitleApp
        {
            get { return _TitleApp; }
            set
            {
                if (_TitleApp != value)
                {
                    _TitleApp = value;
                    OnPropertyChanged();
                }
            }
        }

        internal void ChangeTitleMainWindow(TitleAppContext titleType)
        {
            switch (titleType)
            {
                case TitleAppContext.Overview:
                    TitleApp = "Stations Overview"; 
                    break;
                case TitleAppContext.Home:
                    TitleApp = "Stations Details";
                    break;
                case TitleAppContext.Jobs:
                    TitleApp = "Station Jobs Operations";
                    break;
                case TitleAppContext.Setting:
                    TitleApp = "Stations Settings";
                    break;
                case TitleAppContext.Logs:
                    TitleApp = "Stations Jobs Logs";
                    break;
                default:
                    break;
            }
        }
        #endregion Button Control
    }
}
