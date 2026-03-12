using System;

namespace Soenneker.Blazor.Drawflow.Dtos;

internal class EventLog
{
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;

    public string Message { get; set; } = "";

    public string Type { get; set; } = "info";
}