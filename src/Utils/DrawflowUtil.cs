namespace Soenneker.Blazor.Drawflow.Utils;

internal static class DrawflowUtil
{
    public static (string? uri, string? integrity) GetUriAndIntegrityForStyle(bool useCdn = true,
        string localBasePath = "_content/Soenneker.Blazor.Drawflow/css/")
    {
        return useCdn
            ? ("https://cdn.jsdelivr.net/npm/drawflow@0.0.60/dist/drawflow.min.css", "sha256-V+Wzf3LZX5dZcmPxfvCunwoM17lm4Dm59DUIBA1d7fI=")
            : ($"{localBasePath}drawflow.min.css", null);
    }

    public static (string? uri, string? integrity) GetUriAndIntegrityForScript(bool useCdn = true,
        string localBasePath = "_content/Soenneker.Blazor.Drawflow/js/")
    {
        return useCdn
            ? ("https://cdn.jsdelivr.net/npm/drawflow@0.0.60/dist/drawflow.min.js", "sha256-yvjE8Vt14nFpYy7SEicV5SrrWTC5rcpvUZvYKkhaMeI=")
            : ($"{localBasePath}drawflow.min.js", null);
    }
} 