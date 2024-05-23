using SharedProgram.Models;
using System.IO;
using OperationStatus = SharedProgram.DataTypes.CommonDataType.OperationStatus;

namespace SharedProgram.Shared
{
    public class SharedValues
    {
        public static int Index {  get; set; }

        public static SettingsModel Settings = new();

        public static int NumberOfStation = 4;
        public static string AppName = "DipesLink";

        public static string DeviceTransferName = "DipesLinkDeviceTransfer";


        public static OperationStatus OperStatus = OperationStatus.Stopped;

        public static string ConnectionString = "AccountDB.db";
    }
}
