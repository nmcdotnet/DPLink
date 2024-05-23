using DipesLink.Models;
using SharedProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink.ViewModels
{
    public class ViewModelSharedValues
    {
        public static RunningModel Running = new(1);
        public static SettingsModel Settings = new ();
    }
}
