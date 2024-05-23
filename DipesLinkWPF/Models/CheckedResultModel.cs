using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink.Models
{
    public class CheckedResultModel
    {
      
        public string? Index { get; set; }
        public string? ResultData { get; set; }
        public string? Result {  get; set; }
        public string? ProcessingTime { get; set; }
        public string? DateTime { get; set; }
        public CheckedResultModel(string[] data)
        {
            if (data.Length >= 5)
            {
                Index = data[0];
                ResultData = data[1];
                Result = data[2];
                ProcessingTime = data[3];
                DateTime = data[4];
            }
        
        }

    }
}
