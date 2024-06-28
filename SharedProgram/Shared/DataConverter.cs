using Newtonsoft.Json;
using SharedProgram.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedProgram.Shared
{
    public static class DataConverter
    {


        //        public static byte[] ToByteArray<T>(T data)
        //        {
        //            try
        //            {
        //                string jsonString = JsonConvert.SerializeObject(data); // Tuần tự hóa đối tượng thành chuỗi JSON
        //                byte[] byteArray = Encoding.UTF8.GetBytes(jsonString); // Chuyển chuỗi JSON thành byte array
        //                return byteArray;
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                Console.WriteLine($"ToByteArray fail {ex.Message}");
        //#endif
        //                return Array.Empty<byte>();
        //            }
        //        }

        public static byte[] ToByteArray<T>(T data)
        {
            try
            {
                using var memoryStream = new MemoryStream();
                using (var writer = new Utf8JsonWriter(memoryStream))
                {
                    System.Text.Json.JsonSerializer.Serialize(writer, data);
                }
                byte[] byteArray = memoryStream.ToArray();
                return byteArray;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"ToByteArray fail {ex.Message}");
#endif
                return Array.Empty<byte>();
            }

        }

        //        public static T? FromByteArray<T>(byte[] data)
        //        {
        //            try
        //            {
        //                Utf8JsonReader reader = new(data);
        //                using var doc = JsonDocument.ParseValue(ref reader);
        //                return JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText());
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                Debug.WriteLine($"FromByteArray fail {ex.Message}"); // A000;B000 string[]
        //#endif
        //                return default;
        //            }

        //        }
        //        public static T? FromByteArray<T>(byte[] data)
        //        {
        //            try
        //            {
        //                string jsonString = Encoding.UTF8.GetString(data); // Chuyển byte array thành chuỗi
        //                return JsonSerializer.Deserialize<T>(jsonString); // Giải tuần tự hóa từ chuỗi
        //            }
        //            catch (JsonException jsonEx)
        //            {
        //#if DEBUG
        //                Debug.WriteLine($"FromByteArray JsonException: {jsonEx.Message}");
        //#endif
        //                return default;
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                Debug.WriteLine($"FromByteArray fail {ex.Message}");
        //#endif
        //                return default;
        //            }
        //        }

        //        public static T? FromByteArray<T>(byte[] data)
        //        {
        //            try
        //            {
        //                string jsonString = Encoding.UTF8.GetString(data); // Chuyển byte array thành chuỗi JSON
        //                return JsonConvert.DeserializeObject<T>(jsonString); // Giải tuần tự hóa từ chuỗi JSON
        //            }
        //            catch (Newtonsoft.Json.JsonException jsonEx)
        //            {
        //#if DEBUG
        //                Console.WriteLine($"FromByteArray JsonException: {jsonEx.Message}");
        //#endif
        //                return default;
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                Console.WriteLine($"FromByteArray fail {ex.Message}");
        //#endif
        //                return default;
        //            }
        //        }

        public static T? FromByteArray<T>(byte[] data)
        {
            try
            {
                
                Utf8JsonReader reader = new(data);
                using var doc = JsonDocument.ParseValue(ref reader);
                return System.Text.Json.JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText());
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"FromByteArray fail {ex.Message}"); // A000;B000 string[]
#endif
                return default;
            }

        }


    }
}
