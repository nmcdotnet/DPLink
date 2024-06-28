using Cognex.DataMan.SDK.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProgram.Models
{
    public class CameraInfos
    {
        public int Index { get; set; }
      //  public CameraType CameraType { get; set; }
        public bool ConnectionStatus { get; set; }

        public CamInfo? Info { get; set; }
       
    }
}
