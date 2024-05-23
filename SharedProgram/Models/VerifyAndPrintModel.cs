using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProgram.Models
{
    public class VerifyAndPrintModel
    {
        public bool VerifyAndPrintBasicSentMethod { get; set; }
        public string FailedDataSentToPrinter { get; set; } = string.Empty;
        public List<PODModel> PrintFieldForVerifyAndPrint { get; set; } = new();
    }
}
