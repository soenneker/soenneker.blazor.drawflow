using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Drawflow.Models;

/// <summary>
/// Represents a node within a drawflow module with all its properties and connections
/// </summary>
public sealed class DrawflowNode
{
    /// <summary>
    /// Unique identifier for the node
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Name of the node
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Custom data associated with the node
    /// </summary>
    [JsonPropertyName("data")]
    public Dictionary<string, object>? Data { get; set; }

    /// <summary>
    /// CSS class name for styling the node
    /// </summary>
    [JsonPropertyName("class")]
    public string? Class { get; set; }

    /// <summary>
    /// HTML content for the node display
    /// </summary>
    [JsonPropertyName("html")]
    public string? Html { get; set; }

    /// <summary>
    /// Whether this is a type node (template node)
    /// </summary>
    [JsonPropertyName("typenode")]
    public bool TypeNode { get; set; }

    /// <summary>
    /// Input connections for the node, keyed by input name
    /// </summary>
    [JsonPropertyName("inputs")]
    public Dictionary<string, DrawflowNodeIO>? Inputs { get; set; }

    /// <summary>
    /// Output connections for the node, keyed by output name
    /// </summary>
    [JsonPropertyName("outputs")]
    public Dictionary<string, DrawflowNodeIO>? Outputs { get; set; }

    /// <summary>
    /// X position of the node on the canvas
    /// </summary>
    [JsonPropertyName("pos_x")]
    public int PosX { get; set; }

    /// <summary>
    /// Y position of the node on the canvas
    /// </summary>
    [JsonPropertyName("pos_y")]
    public int PosY { get; set; }

    /// <summary>
    /// Alternative X position (used for some positioning calculations)
    /// </summary>
    [JsonPropertyName("alt_pos_x")]
    public int AltPosX { get; set; }

    /// <summary>
    /// Alternative Y position (used for some positioning calculations)
    /// </summary>
    [JsonPropertyName("alt_pos_y")]
    public int AltPosY { get; set; }
} 