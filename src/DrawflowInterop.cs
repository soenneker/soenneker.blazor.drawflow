using Soenneker.Blazor.Drawflow.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Drawflow;

/// <inheritdoc cref="IDrawflowInterop"/>
public sealed class DrawflowInterop: IDrawflowInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<DrawflowInterop> _logger;

    public DrawflowInterop(IJSRuntime jSRuntime, ILogger<DrawflowInterop> logger)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
    }
}
