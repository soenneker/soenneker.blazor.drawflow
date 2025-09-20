using System;

namespace Soenneker.Blazor.Drawflow.Dtos
{
    internal class EventLog
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Message { get; set; } = "";
        public string Type { get; set; } = "info";
    }
}