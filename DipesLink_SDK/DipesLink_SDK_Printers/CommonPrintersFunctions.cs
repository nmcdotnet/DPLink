using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink_SDK_Printers
{
    public abstract class CommonPrintersFunctions
    {
        public abstract bool Connect();
        public abstract bool Disconnect();
        public abstract void SendData(string data);
    }
}
