using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace DipesLink.Models
{
    public class EventsLogModel
    {
        //{ "Index", "EventType", "Title", "Message", "DateTime" };
        public string? EventType { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? DateTime { get; set; }

        public EventsLogModel(string[] data)
        {
            if (data.Length >= 4)
            {
                EventType = data[0];
                Title = data[1];
                Message = data[2];
                DateTime = data[3];
            }

        }
    }
}
