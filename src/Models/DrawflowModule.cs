using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Drawflow.Models;

/// <summary>
/// Represents a module within the drawflow graph containing nodes
/// </summary>
public class DrawflowModule
{
    /// <summary>
    /// Dictionary of nodes within this module, keyed by node ID
    /// </summary>
    [JsonPropertyName("data")]
    public Dictionary<string, DrawflowNode>? Data { get; set; }
} 