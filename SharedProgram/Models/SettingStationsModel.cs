using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProgram.Models
{
    public class SettingStationsModel :CommonSettingsModel
    {
        public int Index { get; set; }
        public string CameraIP { get; set; } = "";
        public string PrinterIP { get; set; } = "";
        public string PrinterPort { get; set; } = "";
        public string ControllerIP { get; set; } = "";
        public string ControllerPort { get; set; } = "";
        public JobModel Job { get; set; } = new();
    }
}
