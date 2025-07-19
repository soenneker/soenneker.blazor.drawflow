using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Drawflow.Models;

/// <summary>
/// Represents the complete drawflow export structure containing all modules and their data
/// </summary>
public sealed class DrawflowExport
{
    /// <summary>
    /// Dictionary of modules, where each module contains nodes and their data
    /// </summary>
    [JsonPropertyName("drawflow")]
    public Dictionary<string, DrawflowModule>? Drawflow { get; set; }
} 