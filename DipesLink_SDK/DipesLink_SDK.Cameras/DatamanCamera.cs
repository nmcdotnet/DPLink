using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using IPCSharedMemory;
using IPCSharedMemory.Controllers;
using SharedProgram.DeviceTransfer;
using SharedProgram.Models;
using SharedProgram.Shared;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace DipesLink_SDK_Cameras
{
    public class DatamanCamera : CommonCamerasFunctions
    {
        #region Properties and Fields

        private int _index;
        private Thread? _threadCameraStatusChecking;
        private string? _ipAddress;
        private bool _IsConnected;
        public bool IsConnected => _IsConnected;

        private EthSystemDiscoverer? _ethSystemDiscoverer = new();
        internal List<EthSystemDiscoverer.SystemInfo> _cameraSystemInfoList = new();
        private DataManSystem? _dataManSystem;
        private ResultCollector? _results;
        private ISystemConnector? _connector;

        #endregion

        public DatamanCamera(int index)
        {
            _index = index;

            SharedEventsIpc.CameraStatusChanged += SharedEvents_DeviceStatusChanged;
            SharedEvents.OnCameraOutputSignalChange += SharedEvents_OnCameraOutputSignalChange;

            _ethSystemDiscoverer.SystemDiscovered += new EthSystemDiscoverer.SystemDiscoveredHandler(OnEthSystemDiscovered);
            _ethSystemDiscoverer.Discover();

            _threadCameraStatusChecking = new Thread(CameraStatusChecking)
            {
                IsBackground = true,
                Priority = ThreadPriority.Normal
            };
            _threadCameraStatusChecking.Start();
        }

        private void CameraStatusChecking(object? obj)
        {
            while (true)
            {
#if DEBUG
                //Console.WriteLine("Cam Current IP : " + DeviceSharedValues.CameraIP);
#endif
                if (!IsConnected && PingIPCamera(DeviceSharedValues.CameraIP))  // Kiểm tra có IP mới kết nối thử
                {
                    Connect();
                }
                else if (!PingIPCamera(DeviceSharedValues.CameraIP)) // Mất IP sẽ mất kết nối
                {
                    _IsConnected = false;
                    Disconnect();
                    SharedEventsIpc.RaiseCameraStatusChanged(_IsConnected, EventArgs.Empty);
                }
                Thread.Sleep(2000);
            }
        }
        private void SharedEvents_OnCameraOutputSignalChange(object? sender, EventArgs e)
        {
            OutputAction();
        }

        private void SharedEvents_DeviceStatusChanged(object? sender, EventArgs e)
        {
            try
            {
                if (sender == null) return;
                bool camIsConnected = (bool)sender;
                IPCSharedMemory.Datatypes.Enums.CameraStatus camsts;
                if (camIsConnected)
                {
                    camsts = IPCSharedMemory.Datatypes.Enums.CameraStatus.Connected;
                }
                else
                {
                    camsts = IPCSharedMemory.Datatypes.Enums.CameraStatus.Disconnected;
                }
                MemoryTransfer.SendCameraStatusToUI(_index, camsts);
            }
            catch (Exception) { }
        }



     
       
        public override void Connect()
        {
            try
            {

#if DEBUG
                if (DeviceSharedValues.CameraIP == "127.0.0.1")
                {
                    _IsConnected = true;
#if DEBUG
                    Console.WriteLine("Camera is connected !");
#endif
                    SharedEventsIpc.RaiseCameraStatusChanged(_IsConnected, EventArgs.Empty);
                    return;
                }

#endif

                if (_cameraSystemInfoList.Count < 1) return; // nếu chưa có camera nào thì không thực thi tiếp
                //ReleaseCameraResource();
                EthSystemDiscoverer.SystemInfo? currentCamera = _cameraSystemInfoList
                    .Where(x => x.IPAddress.ToString() == DeviceSharedValues.CameraIP)
                    .ToList()
                    .FirstOrDefault(); // lấy ra camera có IP đã set trước
                //Console.WriteLine("Connect IP Cam" +DeviceSharedValues.CameraIP);
                EthSystemConnector? conn = new(currentCamera?.IPAddress);

                if (conn.Address != null)
                {
                    _connector = conn;
                    _dataManSystem = new(_connector) { DefaultTimeout = 1000 }; // Đối tượng đăng sự kiện xử lý chính của camera

                    // sự kiện kết nối và ngắt kết nối
                    
                    _dataManSystem.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
                    _dataManSystem.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);

                    // đăng ký sự kiện lấy ra dữ liệu đọc về gồm xml object chứa thông tin code, Hình ảnh và Graphic trên hình ảnh
                    ResultTypes resultTypes = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;

                    _results = new(_dataManSystem, resultTypes);

                    _results.ComplexResultCompleted += ResultCollector_ComplexResultCompleted;
                    _dataManSystem.Connect(); // bắt đầu kết nối 
                    _dataManSystem.SetResultTypes(resultTypes);
              
                }
                if (conn.Address == null)
                {
                    _dataManSystem?.Dispose();
                    _dataManSystem = null;
                }
            }
            catch (Exception)
            {
#if DEBUG
                Console.WriteLine("Camera connection error !");
#endif
            }
        }

       
        public void Disconnect()
        {
            try
            {
                if (_dataManSystem == null || _dataManSystem.State != ConnectionState.Connected)
                    return;

                _dataManSystem.Disconnect();
                CleanupConnection();

                _results.ClearCachedResults();
                _results = null;
            }
            catch { }
        }
        private void CleanupConnection()
        {
            if (null != _dataManSystem)
            {
                _dataManSystem.SystemConnected -= OnSystemConnected;
                _dataManSystem.SystemDisconnected -= OnSystemDisconnected;
            }

            _connector = null;
            _dataManSystem = null;
        }

        #region Event


        private void OnEthSystemDiscovered(EthSystemDiscoverer.SystemInfo systemInfo)
        {
            bool hasExist = CheckCameraInfoHasExist(systemInfo, _cameraSystemInfoList);
            if (!hasExist)
                _cameraSystemInfoList.Add(systemInfo);
        }

        private readonly object _CurrentResultInfoSyncLock = new();

      
        private void ResultCollector_ComplexResultCompleted(object sender, ComplexResult complexResult)
        {
            ResultCollector resultCollector = (ResultCollector)sender;
            FieldInfo? field = resultCollector.GetType().GetField("_dmSystem", BindingFlags.NonPublic | BindingFlags.Instance);

            System.Drawing.Image? imageResult = null;
            string strResult = "";
            List<string> imageGraphics = new();

            byte[] imageData = Array.Empty<byte>(); ;
            byte[] resultData;

            lock (_CurrentResultInfoSyncLock)
            {
                foreach (SimpleResult simpleResult in complexResult.SimpleResults)
                {
                    switch (simpleResult.Id.Type)
                    {
                        case ResultTypes.Image: // Hình ảnh chụp từ camera
                            imageData = simpleResult.Data;
                            imageResult = SharedFunctions.GetImageFromImageByte(simpleResult.Data);
                            break;

                        case ResultTypes.ImageGraphics: // Hình ảnh polygon xác định đối tượng của camera, ngoài ra còn các thông tin của mã đọc về (lấy gần như đủ)
                            resultData = simpleResult.Data;
                            imageGraphics.Add(simpleResult.GetDataAsString());
                            break;

                        case ResultTypes.ReadString: // Dữ liệu đọc về từ code
                            strResult = Encoding.UTF8.GetString(simpleResult.Data);
                            break;

                        case ResultTypes.ReadXml: //  Dữ liệu đọc về từ code (lấy từ xml)
                            strResult = SharedFunctions.GetReadStringFromResultXml(simpleResult.GetDataAsString());
                            break;

                        default:
                            break;
                    }
                }
            }
           // Console.WriteLine("Co Chup Anh");
            var points = GetResultFromXmlString(imageGraphics.FirstOrDefault()); // Get polygon result
            DetectModel detectModel = new()
            {
                Text = Regex.Replace(strResult, @"\r\n", ""),  // Replace special characters in camera data by symbol ';'
                ImagePolyResult = points,
                ImageData = imageData,
                Image = GetImageWithFocusRectangle(imageResult, imageGraphics)
            };
            Console.WriteLine("Capture Image !");
            SharedEvents.RaiseOnCameraReadDataChangeEvent(detectModel); // Send data via Event
        }

        private static Bitmap GetImageWithFocusRectangle(Image? imageResult, List<string> imageGraphics)
        {
            Bitmap bitmap = new(100, 100);
            try
            {
                if (imageResult != null)
                {
                    bitmap = ((Bitmap)imageResult).Clone(new System.Drawing.Rectangle(0, 0, imageResult.Width, imageResult.Height), PixelFormat.Format24bppRgb);
                }
                else
                {
                    using var g = Graphics.FromImage(bitmap);
                    g.Clear(Color.White);
                }
                if (imageGraphics.Count > 0)
                {
                    using var graphicsImage = Graphics.FromImage(bitmap);
                    foreach (string graphics in imageGraphics)
                    {
                        ResultGraphics resultGraphics = GraphicsResultParser.Parse(graphics, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height));
                        ResultGraphicsRenderer.PaintResults(graphicsImage, resultGraphics);
                    }
                }
                return bitmap.Clone(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
            }
            catch (Exception) { return bitmap; }
        }

        private string? GetResultFromXmlString(string? xmlString)
        {
            if (xmlString == null) return null;
            XDocument xmlDoc = XDocument.Parse(xmlString);
            XNamespace ns = "http://www.w3.org/2000/svg"; // For Sgv Format namespace
            IEnumerable<XElement> polygons = xmlDoc
                        .Descendants(ns + "polygon")
                        .Where(p => (string?)p
                        .Attribute("class") == "result");
            string? pointsValue = polygons.Select(x => x.Attribute("points"))?.FirstOrDefault()?.Value.ToString(); // Get Points value
            return pointsValue;
        }

        private void OnSystemDisconnected(object sender, EventArgs args)
        {
#if DEBUG
            Console.WriteLine("Camera is disconnected !");
#endif
            if (sender is DataManSystem)
            {
                DataManSystem? dataManSystem = sender as DataManSystem;
                if (dataManSystem?.Connector is EthSystemConnector ethSystemConnector)
                {
                    _IsConnected = false;
                    SharedEventsIpc.RaiseCameraStatusChanged(_IsConnected, EventArgs.Empty);
                }
            }
        }

        private void OnSystemConnected(object sender, EventArgs args)
        {

#if DEBUG
            Console.WriteLine("Camera is connected !");
#endif

            if (sender is DataManSystem)
            {
                DataManSystem? dataManSystem = sender as DataManSystem;
                if (dataManSystem?.Connector is EthSystemConnector ethSystemConnector)
                {
                    _IsConnected = true;
                    SharedEventsIpc.RaiseCameraStatusChanged(_IsConnected, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Functions
       


        private static bool PingIPCamera(string ipAddress) // Hàm kiểm tra địa chỉ IP có tồn tại hay không
        {
            try
            {
                Ping pingSender = new();
                PingReply reply = pingSender.Send(ipAddress);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Kiểm tra camera có tồn tại trong list hay chưa dựa vào Serial Number
        /// </summary>
        /// <param name="cameraInfoNeedCheck"></param>
        /// <param name="cameraSystemInfoList"></param>
        /// <returns></returns>
        private static bool CheckCameraInfoHasExist(EthSystemDiscoverer.SystemInfo cameraInfoNeedCheck, List<EthSystemDiscoverer.SystemInfo> cameraSystemInfoList)
        {
            foreach (object systemInfo in cameraSystemInfoList)
            {
                if (systemInfo is EthSystemDiscoverer.SystemInfo)
                {
                    EthSystemDiscoverer.SystemInfo? ethSystemInfo = systemInfo as EthSystemDiscoverer.SystemInfo;
                    if (ethSystemInfo?.SerialNumber == cameraInfoNeedCheck.SerialNumber)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Operation

        /// <summary>
        /// Trigger manual take a photo
        /// </summary>
        /// <returns></returns>
        public override bool ManualInputTrigger()
        {
            try
            {
                DmccResponse? response = _dataManSystem?.SendCommand("TRIGGER ON");
#if DEBUG
                //  Console.WriteLine(response?.PayLoad);
#endif
                return true;
            }
            catch (Exception) { return false; }
        }

        /// <summary>
        /// Trigger USER1 output
        /// </summary>
        /// <returns></returns>
        public override bool OutputAction()
        {
            try
            {
                DmccResponse? response = _dataManSystem?.SendCommand("OUTPUT.USER1");
                return true;
            }
            catch (Exception) { return false; }
        }

        #endregion

    }
}
