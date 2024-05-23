using static IPCSharedMemory.Datatypes.Enums;
using static SharedProgram.DataTypes.CommonDataType;

namespace IPCSharedMemory
{
    public class MemoryTransfer
    {
        #region Procedural Function
        public static void SendCommandToUI(int index, byte[] command, long capacity = 1024*1024*1) // 1MB
        {
                using IPCSharedHelper ipc = new(index, "DeviceToUISharedMemory", capacity);
                ipc.SendData(command);
        }

        public static void SendDatabaseCommandToUI(int index, byte[] command, long capacity = 1024 * 1024 * 200) // 200MB
        {
            using IPCSharedHelper ipc = new(index, "DeviceToUISharedMemory_DB", capacity); // Printed List DB
            ipc.SendData(command);
        }

        public static void SendCheckedDatabaseCommandToUI(int index, byte[] command, long capacity = 1024 * 1024 * 200) // 200MB
        {
            using IPCSharedHelper ipc = new(index, "DeviceToUISharedMemory_CheckedDB", capacity); // Checked List DB
            ipc.SendData(command);
        }

        public static void SendRealTimeDataCommandToUI(int index, byte[] command, long capacity = 1024 * 1024 * 100) // 1MB
        {
            using IPCSharedHelper ipc = new(index, "DeviceToUISharedMemory_RD", capacity);
            ipc.SendData(command);
        }

        public static void SendCommandToDevice(int index, byte[] command)
        {
            Thread thread = new(() =>
            {
                using IPCSharedHelper ipc = new(index, "UIToDeviceSharedMemory");
                ipc.SendData(command);
            })
            { IsBackground = true, Priority = ThreadPriority.Highest };
            thread.Start();
        }
        #endregion Procedural Function

        #region IPC UI -> Device Transfer

        /// <summary>
        /// Send connection setting parameter from UI to Device Transfer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public static void SendConnectionParamsToDevice(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.UICommand ,
                    (byte)index,
                    (byte)SharedMemoryType.ParamsSetting,
                }; // This is header

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToDevice(index, newCommand); // send memory map file
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Send Button Action Type : Start , Stop, Trigger from  UI to Device Transfer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public static void SendActionButtonToDevice(int index, byte[] data)
        {
            try
            {
                byte[] command =
                {
                    (byte)SharedMemoryCommandType.UICommand,
                    (byte)index,
                    (byte)SharedMemoryType.ActionButton,
                };

                var newCommand = new byte[command.Length + data.Length];
                Array.Copy(command, 0, newCommand, 0, command.Length);
                Array.Copy(data, 0, newCommand, command.Length, data.Length);
                SendCommandToDevice(index, newCommand);
            }
            catch (Exception) { }
        }

        #endregion IPC UI -> Device Transfer



        #region IPC Device Transfer -> UI

        public static void SendDatabaseToUIFirstTime(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.DatabaseList,
                };
                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1
                                                                              // Console.WriteLine("Send: "+ newCommand.Length);
                SendDatabaseCommandToUI(index, newCommand,1024*1024*200); // send memory map file
            }
            catch (Exception) { }
        }


        public static void SendCheckedDatabaseToUIFirstTime(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.CheckedList,
                };
                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1
                                                                              // Console.WriteLine("Send: "+ newCommand.Length);
                SendCheckedDatabaseCommandToUI(index, newCommand, 1024 * 1024 * 200); // send memory map file
            }
            catch (Exception) { }
        }

        //public static void SendDatabaseToUIFirstTime(int index, byte[] data)
        //{

        //}

        /// <summary>
        /// Send Printer Connection status from Device Transfer to UI
        /// </summary>
        /// <param name="index"></param>
        /// <param name="printerStatus"></param>
        public static void SendPrinterStatusToUI(int index, PrinterStatus printerStatus)
        {
            try
            {
#if DEBUG
               // Console.WriteLine("Printer Status: " + printerStatus);
#endif
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.PrinterStatus,
                    (byte)printerStatus
                };
                SendCommandToUI(index, command);
            }
            catch (Exception) { }
        }

        public static void SendControllerStatusToUI(int index, ControllerStatus controllerStatus)
        {
            try
            {
#if DEBUG
                // Console.WriteLine("Printer Status: " + printerStatus);
#endif
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.ControllerStatus,
                    (byte)controllerStatus
                };
                SendCommandToUI(index, command);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Send Camera Connection status from Device Transfer to UI
        /// </summary>
        /// <param name="index"></param>
        /// <param name="camStatus"></param>
        public static void SendCameraStatusToUI(int index, CameraStatus camStatus)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.CamStatus,
                    (byte)camStatus
                };
                SendCommandToUI(index, command);
            }
            catch (Exception) { }
        }

        public static void SendControllerResponseMessageToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.ControllerResponseMess,
                };
                var newCommand = new byte[command.Length + data.Length]; 
                Array.Copy(command, 0, newCommand, 0, command.Length);
                Array.Copy(data, 0, newCommand, command.Length, data.Length);
                Console.WriteLine("DataLenght: "+ command.Length);
                SendCommandToUI(index, newCommand);
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Send printer template list from Device transfer to UI 
        /// </summary>
        public static void SendPrinterTemplateListToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.PrinterTemplate,
                };
                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception){}
        }

        #region Sent/Received/Printed Counter
        //Sent Number
        public static void SendCounterSentToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.StatisticsCounterSent,
                };

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception) { }
        }

        //Received Number
        public static void SendCounterReceivedToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.StatisticsCounterReceived,
                };

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception) { }
        }

        //Printed Number
        public static void SendCounterPrintedToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.StatisticsCounterPrinted,
                };

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception) { }
        }
        #endregion end Sent/Received/Printed Counter


        public static void SendCurrentPosDatabaseToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.CurrentPosDb,
                };

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception) { }
        }

        public static void SendDetectModelToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.DetectModel,
                };
                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendRealTimeDataCommandToUI(index, newCommand);

            }
            catch (Exception)
            {
            }
        }

        public static void SendCheckedStatisticsToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.CheckedStatistics,
                };
                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendRealTimeDataCommandToUI(index, newCommand);
            }
            catch (Exception)
            {
            }
        }

        public static void SendCheckedResultRawToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.CheckedResultRaw,
                };

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendRealTimeDataCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception) { }
        }

        public static void SendPrintedCodeRawToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.PrintedCodeRaw,
                };

                var newCommand = new byte[command.Length + data.Length]; // Create new array
                Array.Copy(command, 0, newCommand, 0, command.Length); // copy array 1 to new array
                Array.Copy(data, 0, newCommand, command.Length, data.Length); // copy array 2 to new array start from length array 1

                SendCommandToUI(index, newCommand); // send memory map file
            }
            catch (Exception){ }
        }

        public static void SendOperationStatusToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.JobOperationStatus
                };

                var newCommand = new byte[command.Length + data.Length]; 
                Array.Copy(command, 0, newCommand, 0, command.Length); 
                Array.Copy(data, 0, newCommand, command.Length, data.Length); 

                SendCommandToUI(index, newCommand); 
            }
            catch (Exception) { }
        }

        public static void SendMessageJobStatusToUI(int index, byte[] data)
        {
            try
            {
                byte[] command = {
                    (byte)SharedMemoryCommandType.DeviceCommand ,
                    (byte)index,
                    (byte)SharedMemoryType.JobMessageStatus,
                };

                var newCommand = new byte[command.Length + data.Length];
                Array.Copy(command, 0, newCommand, 0, command.Length);
                Array.Copy(data, 0, newCommand, command.Length, data.Length);

                SendCommandToUI(index, newCommand);
            }
            catch (Exception) { }
        }


        #endregion IPC Device Transfer -> UI


    }
}
