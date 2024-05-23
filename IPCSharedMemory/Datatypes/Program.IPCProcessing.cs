using IPCSharedMemory;
using SharedProgram.DeviceTransfer;
using System.Drawing.Printing;
using static IPCSharedMemory.Datatypes.Enums;

namespace DipesLinkDeviceTransfer
{
    /// <summary>
    /// IPC Memory Name distinguished by indexes
    /// </summary>
    /// 
    public partial class Program
    {
        //private void ListenConnectionParam(int index)
        //{
        //    Task.Run(async () =>
        //    {
        //        using IPCSharedHelper ipc = new(index, "UIToDeviceSharedMemory", isReceiver: true);
        //        while (true)
        //        {
        //            bool isCompleteDequeue = ipc.MessageQueue.TryDequeue(out byte[]? result);
        //            if (isCompleteDequeue && result != null)
        //            {
        //                switch (result[0])
        //                {
        //                    case (byte)SharedMemoryCommandType.UICommand:
        //                        switch (result[2])
        //                        {
        //                            // Get Params Setting for Connection (IP, Port) device
        //                            case (byte)SharedMemoryType.ParamsSetting:
        //                                var resultParams = new byte[result.Length - 3]; // reject header
        //                                Array.Copy(result, 3, resultParams, 0, resultParams.Length);
        //                                DeviceSharedFunctions.GetConnectionParamsSetting(resultParams);
        //                                break;

        //                            // Get Action Button Type (Start , Stop, Trigger) signal 
        //                            case (byte)SharedMemoryType.ActionButton:
        //                                var resultActionButton = new byte[result.Length - 3];
        //                                Array.Copy(result, 3, resultActionButton, 0, resultActionButton.Length);
        //                                DeviceSharedFunctions.GetActionButton(resultActionButton);
        //                                ActionButtonFromUIProcessingAsync();
        //                                break;
        //                        }
        //                        break;
        //                }
        //            }
        //             await Task.Delay(1);
        //        }
        //    });
        //}

        //private static async void ActionButtonFromUIProcessingAsync()
        //{
        //    await Task.Run(() =>
        //    {
        //        switch (DeviceSharedValues.ActionButtonType)
        //        {
        //            case "1":  // Start
        //              //  StartProcess();
                     
        //                break;
        //            case "2": //Stop 

        //                break;
        //            case "3": // Trigger

        //                break;
        //            case "4": // Reload template
        //              //  ObtainPrintProductTemplateList();
        //                break;
        //            default:
        //                break;
        //        }
        //    });
        //}
        

       

    }
}
