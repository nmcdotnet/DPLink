using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink_SDK_Cameras.Models
{
    public class Camera
    {
        public int Index { get; set; }
        public CameraType CameraType { get; set; }
        public string? IPAddress { get; set; }
        public string? SerialNumber { get; set; }
        public bool ConnectionStatus { get; set; }
    }
}
