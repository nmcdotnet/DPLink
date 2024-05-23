using System.Text.Json;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConsoleAppTestFunction
{
    public static class NetworkUtils
    {
        public static bool IsPortOpen(string ipAddress, int port, int timeout = 1000)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    var result = tcpClient.BeginConnect(ipAddress, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeout));
                    if (!success)
                    {
                        return false;
                    }

                    tcpClient.EndConnect(result);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
            string ipAddress = "127.0.0.1"; // Localhost
            int port = 2001; // HTTP port
            bool isPortOpen = NetworkUtils.IsPortOpen(ipAddress, port);
            Console.WriteLine($"Port {port} on {ipAddress} is open: {isPortOpen}");
        }
    }
}
