using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProgram.Controller
{
    public class CompareStatus
    {
        public CompareStatus()
        {
            Index = -1;
            Status = false;
            DuplicateId = new List<int>();
        }

        public CompareStatus(int index, bool status)
        {
            Index = index;
            Status = status;
            DuplicateId = new List<int>();
        }

        public int Index { get; set; }

        public bool Status { get; set; }

        public List<int> DuplicateId { get; set; }
    }
}
