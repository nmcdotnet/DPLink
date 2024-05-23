using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink.Models
{
    public class EventArgumentCustom : EventArgs
    {
        public object? Data { get; set; }
    }
}
