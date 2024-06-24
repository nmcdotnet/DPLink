using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink.ViewModels
{
    public class ViewModelSharedEvents
    {
        public static event EventHandler? MainListBoxMenuChange;
        public static void OnMainListBoxMenuChange()
        {
            MainListBoxMenuChange?.Invoke(null, EventArgs.Empty);
        }


        public static event EventHandler<int>? OnJobDetailChange; // event station detail usercontrol change
        public static void OnJobDetailChangeHandler(int currentJob)
        {
            OnJobDetailChange?.Invoke(null, currentJob);
        }
    }
}
