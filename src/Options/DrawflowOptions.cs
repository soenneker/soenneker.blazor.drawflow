namespace Soenneker.Blazor.Drawflow.Options;

/// <summary>
/// Options for initializing a Drawflow instance.
/// These mirror common properties found in the JavaScript library.
/// </summary>
public class DrawflowOptions
{
    /// <summary>Enables connection rerouting.</summary>
    public bool Reroute { get; set; } = false;

    public bool RerouteFixCurvature { get; set; } = false;

    public double Curvature { get; set; } = 0.5;
    public double RerouteCurvatureStartEnd { get; set; } = 0.5;
    public double RerouteCurvature { get; set; } = 0.5;
    public int RerouteWidth { get; set; } = 6;
    public int LinePath { get; set; } = 5;
    public bool ForceFirstInput { get; set; } = false;
    public string EditorMode { get; set; } = "edit";
    public double Zoom { get; set; } = 1;
    public double ZoomMax { get; set; } = 1.6;
    public double ZoomMin { get; set; } = 0.5;
    public double ZoomValue { get; set; } = 0.1;
    public double ZoomLastValue { get; set; } = 1;
    public bool DraggableInputs { get; set; } = true;
    public bool UseUuid { get; set; } = false;

    /// <summary>
    /// Whether to use CDN for loading Drawflow resources. Defaults to true.
    /// </summary>
    public bool UseCdn { get; set; } = true;

    /// <summary>
    /// Whether to manually create the Drawflow instance. If true, you must call Create() manually.
    /// </summary>
    public bool ManualCreate { get; set; } = false;
}
