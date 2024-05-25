using DipesLink_SDK_Cameras;
using DipesLink_SDK_PLC;
using DipesLink_SDK_Printers;
using SharedProgram.DeviceTransfer;
using SharedProgram.Models;
using SharedProgram.Shared;

namespace DipesLinkDeviceTransfer
{
    public partial class Program
    {
        public static DatamanCamera? DatamanCameraDeviceHandler;
        public static RynanRPrinterTCPClient? RynanRPrinterDeviceHandler;
        public static S7TCPIP? ControllerDeviceHandler;
        public static char keyStep;

        private static void GetArgumentList(string[] args)
        {
            try
            {
                DeviceSharedValues.Index = int.Parse(args[0]);
                //DeviceSharedValues.CameraIP = args[1];
                //DeviceSharedValues.PrinterIP = args[2];
                //DeviceSharedValues.PrinterPort = args[3];
            }
            catch (Exception)  // for Device transfer only 
            {
                DeviceSharedValues.Index = 0;
                DeviceSharedValues.CameraIP = "192.168.253.23";
                DeviceSharedValues.PrinterIP = "192.168.253.3"; //192.168.3.52
                DeviceSharedValues.PrinterPort = "2030";
                DeviceSharedValues.ControllerIP = "127.0.0.1";
                DeviceSharedValues.ControllerPort = "2001";
                DeviceSharedValues.VPObject = new()
                {
                    FailedDataSentToPrinter = "Failure",
                    VerifyAndPrintBasicSentMethod = false,
                    PrintFieldForVerifyAndPrint = new()
                    {
                        new() { Index = 1,Type=PODModel.TypePOD.FIELD, PODName="", Value=""}
                    }
                };
               
            }
        }


        static void Main(string[] args)
        {

#if DEBUG
            //Console.WriteLine("List of Argument: ");
            //foreach (var arg in args)
            //{
            //    Console.WriteLine(arg);
            //}
#endif

            GetArgumentList(args);
            JobIndex = DeviceSharedValues.Index;

            new Program().NonStaticMainProgram();

            #region Hold control Readkey
            //Don't exit the Control window when pressing a key
            //Console.ReadKey();
            Thread consoleReadThread = new(() =>
            {
                while (true)
                {
                    var key = Console.ReadKey();
                    if (key.KeyChar == 's')
                    {
                        keyStep = 's';
                    }
                    if (key.KeyChar == 'v')
                    {
                        keyStep = 'v';
                    }
                    if (key.KeyChar == 'f')
                    {
                        keyStep = 'f';
                    }
                    if (key.KeyChar == 'd')
                    {
                        keyStep = 'd';
                    }
                    if (key.KeyChar == 'n')
                    {
                        keyStep = 'n';
                    }
                    if (key.KeyChar == 'e')
                    {
                        keyStep = 'e';
                    }
                    Thread.Sleep(10);
                }
            });
            consoleReadThread.Start();
            #endregion
        }

        private void ProcessExitHandler(object? sender, EventArgs e)
        {
            ReleaseResource();
        }

        public async void NonStaticMainProgram()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProcessExitHandler);
            ListenConnectionParam(JobIndex);
            AlwaySendPrinterOperationToUI();
            DatamanCameraDeviceHandler = new(JobIndex);
            RynanRPrinterDeviceHandler = new(JobIndex);
            ControllerDeviceHandler = new(JobIndex);
            

            InitEvents();
            //  LoadSelectedJob();

            Thread t = new(() =>
            {
                while (true)
                {
                    if (keyStep == 's')
                    {
                        DeviceSharedValues.ActionButtonType = SharedProgram.DataTypes.CommonDataType.ActionButtonType.Start;
                        ActionButtonFromUIProcessingAsync();
                        keyStep = '\0';
                    }
                    if (keyStep == 'v')
                    {
                        SimulateValidDataCamera();
                        keyStep = '\0';
                    }
                    if (keyStep == 'f')
                    {
                        SimulateInvalidDataCamera(TypeOfSimulateInvalidDataCamera.Fail);
                        keyStep = '\0';
                    }
                    if (keyStep == 'd')
                    {
                        SimulateInvalidDataCamera(TypeOfSimulateInvalidDataCamera.Duplicate);
                        keyStep = '\0';
                    }
                    if ((keyStep == 'n'))
                    {
                        SimulateInvalidDataCamera(TypeOfSimulateInvalidDataCamera.Null);
                        keyStep = '\0';
                    }
                    if ((keyStep == 'e'))
                    {
                        _ = StopProcessAsync();
                        keyStep = '\0';
                    }
                    Thread.Sleep(1000);
                }
            });

            t.Start();
        }

        public Task<int> CheckDeviceConnectionAsync() // 0 OK, 1 Cam connect fail, 2 Printer connect fail
        {
            if (DatamanCameraDeviceHandler != null && !DatamanCameraDeviceHandler.IsConnected)
                return Task.FromResult(1);
            
            if (RynanRPrinterDeviceHandler != null && !RynanRPrinterDeviceHandler.IsConnected())
                return Task.FromResult(2);
            
            return Task.FromResult(0);
        }

        public void InitEvents()
        {
            //SharedPaths.InitCommonPathByIndex(JobIndex);
            PrinterEventInit();
            CameraEventInit();
       
        }


        public void SimulateJobInformation(JobModel jobIn, out JobModel jobOut)
        {
            jobIn.Name = "MyJob";
            jobIn.DatabasePath = "D:\\Program\\DPLink\\SampleDB\\OneMillion.csv";
            jobIn.CheckedResultPath = "240313163302_DPLINK_AfterProduction_Template.csv";
            jobIn.PrintedResponePath = "240313163303_Printed_DPLINK_AfterProduction_Template.csv";
            jobIn.PrinterTemplate = "podtest";
            jobIn.PODFormat = new List<PODModel>
            {
               new() { Index = 1, PODName="Data" ,Type = PODModel.TypePOD.FIELD,Value ="" }
            };
            jobOut = jobIn;
        }

    }
}
