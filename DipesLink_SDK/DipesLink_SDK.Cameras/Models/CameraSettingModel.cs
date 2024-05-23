using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipesLink_SDK_Cameras.Models
{
    public class CameraSettingModel
    {
		private int _index;
		public int Index
		{
			get { return _index; }
			set { _index = value; }
		}

		private string _ipAddress;
		public string IPAddress
		{
			get { return _ipAddress; }
			set { _ipAddress = value; }
		}

		private string _port;
		public string Port
		{
			get { return _port; }
			set { _port = value; }
		}



	}
}
