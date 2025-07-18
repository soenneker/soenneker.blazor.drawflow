using System.Text.Json.Serialization;

namespace Soenneker.Blazor.Drawflow.Options;

/// <summary>
/// Options for initializing a Drawflow instance.
/// These mirror common properties found in the JavaScript library.
/// </summary>
public sealed class DrawflowOptions
{
    [JsonPropertyName("reroute")]
    public bool Reroute { get; set; } = false;

    [JsonPropertyName("reroute_fix_curvature")]
    public bool RerouteFixCurvature { get; set; } = false;

    [JsonPropertyName("curvature")]
    public double Curvature { get; set; } = 0.5;

    [JsonPropertyName("reroute_curvature_start_end")]
    public double RerouteCurvatureStartEnd { get; set; } = 0.5;

    [JsonPropertyName("reroute_curvature")]
    public double RerouteCurvature { get; set; } = 0.5;

    [JsonPropertyName("reroute_width")]
    public int RerouteWidth { get; set; } = 6;

    [JsonPropertyName("line_path")]
    public int LinePath { get; set; } = 5;

    [JsonPropertyName("force_first_input")]
    public bool ForceFirstInput { get; set; } = false;

    [JsonPropertyName("editor_mode")]
    public string EditorMode { get; set; } = "edit";

    [JsonPropertyName("zoom")]
    public double Zoom { get; set; } = 1;

    [JsonPropertyName("zoom_max")]
    public double ZoomMax { get; set; } = 1.6;

    [JsonPropertyName("zoom_min")]
    public double ZoomMin { get; set; } = 0.5;

    [JsonPropertyName("zoom_value")]
    public double ZoomValue { get; set; } = 0.1;

    [JsonPropertyName("zoom_last_value")]
    public double ZoomLastValue { get; set; } = 1;

    [JsonPropertyName("draggable_inputs")]
    public bool DraggableInputs { get; set; } = true;

    [JsonPropertyName("useuuid")]
    public bool UseUuid { get; set; } = true;

    // The following are not part of the official Drawflow options, but may be useful for Blazor interop
    [JsonPropertyName("useCdn")]
    public bool UseCdn { get; set; } = true;

    [JsonPropertyName("manualCreate")]
    public bool ManualCreate { get; set; } = false;
}