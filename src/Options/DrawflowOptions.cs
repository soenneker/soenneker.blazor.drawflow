using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Drawflow.Options;

/// <summary>
/// Options for initializing a Drawflow instance.
/// These mirror common properties found in the JavaScript library.
/// </summary>
public sealed class DrawflowOptions
{
    /// <summary>
    /// Enable automatic rerouting of connections when nodes are moved
    /// </summary>
    [JsonPropertyName("reroute")]
    public bool Reroute { get; set; } = false;

    /// <summary>
    /// Fix curvature when rerouting connections
    /// </summary>
    [JsonPropertyName("reroute_fix_curvature")]
    public bool RerouteFixCurvature { get; set; } = false;

    /// <summary>
    /// Curvature of connection lines (0.0 to 1.0)
    /// </summary>
    [JsonPropertyName("curvature")]
    public double Curvature { get; set; } = 0.5;

    /// <summary>
    /// Curvature at start and end of rerouted connections
    /// </summary>
    [JsonPropertyName("reroute_curvature_start_end")]
    public double RerouteCurvatureStartEnd { get; set; } = 0.5;

    /// <summary>
    /// Curvature for rerouted connections
    /// </summary>
    [JsonPropertyName("reroute_curvature")]
    public double RerouteCurvature { get; set; } = 0.5;

    /// <summary>
    /// Width of rerouted connection lines
    /// </summary>
    [JsonPropertyName("reroute_width")]
    public int RerouteWidth { get; set; } = 6;

    /// <summary>
    /// Path type for connection lines
    /// </summary>
    [JsonPropertyName("line_path")]
    public int LinePath { get; set; } = 5;

    /// <summary>
    /// Force the first input to be connected
    /// </summary>
    [JsonPropertyName("force_first_input")]
    public bool ForceFirstInput { get; set; } = false;

    /// <summary>
    /// Editor mode: "edit" or "fixed"
    /// </summary>
    [JsonPropertyName("editor_mode")]
    public string EditorMode { get; set; } = "edit";

    /// <summary>
    /// Initial zoom level (0.1 to 2.0)
    /// </summary>
    [JsonPropertyName("zoom")]
    public double Zoom { get; set; } = 1;

    /// <summary>
    /// Maximum zoom level
    /// </summary>
    [JsonPropertyName("zoom_max")]
    public double ZoomMax { get; set; } = 1.6;

    /// <summary>
    /// Minimum zoom level
    /// </summary>
    [JsonPropertyName("zoom_min")]
    public double ZoomMin { get; set; } = 0.5;

    /// <summary>
    /// Zoom increment value
    /// </summary>
    [JsonPropertyName("zoom_value")]
    public double ZoomValue { get; set; } = 0.1;

    /// <summary>
    /// Last zoom value used
    /// </summary>
    [JsonPropertyName("zoom_last_value")]
    public double ZoomLastValue { get; set; } = 1;

    /// <summary>
    /// Allow dragging of input connections
    /// </summary>
    [JsonPropertyName("draggable_inputs")]
    public bool DraggableInputs { get; set; } = true;

    /// <summary>
    /// Use UUID for node IDs instead of sequential numbers
    /// </summary>
    [JsonPropertyName("useuuid")]
    public bool UseUuid { get; set; } = true;

    /// <summary>
    /// Whether to load Drawflow resources from CDN or local files
    /// </summary>
    [JsonPropertyName("useCdn")]
    public bool UseCdn { get; set; } = true;

    /// <summary>
    /// Whether to manually create the Drawflow instance (false = auto-create on render)
    /// </summary>
    [JsonPropertyName("manualCreate")]
    public bool ManualCreate { get; set; } = false;
}
