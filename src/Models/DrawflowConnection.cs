using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Drawflow.Models;

/// <summary>
/// Represents a connection between nodes
/// </summary>
public sealed class DrawflowConnection
{
    /// <summary>
    /// The node ID this connection is connected to
    /// </summary>
    [JsonPropertyName("node")]
    public string? Node { get; set; }

    /// <summary>
    /// The input identifier (used for output connections)
    /// </summary>
    [JsonPropertyName("input")]
    public string? Input { get; set; }

    /// <summary>
    /// The output identifier (used for input connections)
    /// </summary>
    [JsonPropertyName("output")]
    public string? Output { get; set; }
} 