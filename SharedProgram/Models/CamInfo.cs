using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProgram.Models
{
    public class CamInfo
    {
        public string? SerialNumber { get; set; }
        public string? IPAddress { get; set; }
        public string? SubnetMask { get; set; }
        public string? Port { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}
