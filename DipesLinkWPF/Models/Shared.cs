using DipesLink.Properties;
using SharedProgram;
using SharedProgram.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink.Models
{
    public class Shared
    {
        public static SettingsModel settings { get; set; }

        public static event EventHandler? OnActionLoadingSplashScreen;
        public static void RaiseLoadingSplashScreen(object obj)
        {
            OnActionLoadingSplashScreen?.Invoke(obj, EventArgs.Empty);
        }


        ///// <summary>
        ///// Event for loaded printed Database (working database)
        ///// </summary>
        //public static event EventHandler? OnLoadCompleteDatabase;
        //public static void RaiseLoadCompleteDatabase(object database)
        //{
        //    OnLoadCompleteDatabase?.Invoke(database, EventArgs.Empty);
        //}

        ///// <summary>
        ///// Event for Loaded Checked Database
        ///// </summary>
        //public static event EventHandler? OnLoadCompleteCheckedDatabase;
        //public static void RaiseLoadCompleteCheckedDatabase(object database)
        //{
        //    OnLoadCompleteCheckedDatabase?.Invoke(database, EventArgs.Empty);
        //}

        ///// <summary>
        ///// Event for receive current printed code 
        ///// </summary>
        //public static event EventHandler? OnChangePrintedCode;
        //public static void RaiseChangePrintedCode(object printedCode)
        //{
        //    OnChangePrintedCode?.Invoke(printedCode, EventArgs.Empty);
        //}

        ///// <summary>
        /////  Event for receive current checked code 
        ///// </summary>
        //public static event EventHandler? OnChangeCheckedCode;
        //public static void RaiseChangeCheckedCode(object checkedCode)
        //{
        //    OnChangeCheckedCode?.Invoke(checkedCode, EventArgs.Empty);
        //}
    }
}
