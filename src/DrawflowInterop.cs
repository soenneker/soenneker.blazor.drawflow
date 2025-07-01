using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Blazor.Drawflow.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Drawflow;

/// <inheritdoc cref="IDrawflowInterop"/>
public sealed class DrawflowInterop : IDrawflowInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<DrawflowInterop> _logger;
    private IJSObjectReference? _module;

    public DrawflowInterop(IJSRuntime jsRuntime, ILogger<DrawflowInterop> logger)
    {
        _jsRuntime = jsRuntime;
        _logger = logger;
    }

    private async ValueTask<IJSObjectReference> Module()
    {
        if (_module is null)
        {
            _module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Soenneker.Blazor.Drawflow/js/drawflowinterop.js");
        }
        return _module;
    }

    public async ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("create", cancellationToken, elementId, options);
    }

    public async ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("addNode", cancellationToken, elementId, name, inputs, outputs, posX, posY, className, data, html);
    }

    public async ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("removeNode", cancellationToken, elementId, nodeId);
    }

    public async ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("addConnection", cancellationToken, elementId, outputNode, inputNode, outputClass, inputClass);
    }

    public async ValueTask<string> Export(string elementId, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        return await module.InvokeAsync<string>("export", cancellationToken, elementId);
    }

    public async ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("import", cancellationToken, elementId, json);
    }

    public async ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        if (_module is null)
            return;
        await _module.InvokeVoidAsync("destroy", cancellationToken, elementId);
    }

    public async ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        var dotNet = DotNetObjectReference.Create(new CallbackInvoker(callback));
        await module.InvokeVoidAsync("addEventListener", cancellationToken, elementId, eventName, dotNet);
    }

    public async ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("zoomIn", cancellationToken, elementId);
    }

    public async ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("zoomOut", cancellationToken, elementId);
    }

    public async ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("addModule", cancellationToken, elementId, name);
    }

    public async ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("changeModule", cancellationToken, elementId, name);
    }

    public async ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("removeModule", cancellationToken, elementId, name);
    }

    public async ValueTask<IJSObjectReference?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        return await module.InvokeAsync<IJSObjectReference?>("getNodeFromId", cancellationToken, elementId, id);
    }

    public async ValueTask<int[]> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        return await module.InvokeAsync<int[]>("getNodesFromName", cancellationToken, elementId, name);
    }

    public async ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("updateNodeData", cancellationToken, elementId, id, data);
    }

    public async ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("addNodeInput", cancellationToken, elementId, id);
    }

    public async ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("addNodeOutput", cancellationToken, elementId, id);
    }

    public async ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("removeNodeInput", cancellationToken, elementId, id, inputClass);
    }

    public async ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("removeNodeOutput", cancellationToken, elementId, id, outputClass);
    }

    public async ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("removeSingleConnection", cancellationToken, elementId, outId, inId, outClass, inClass);
    }

    public async ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("updateConnectionNodes", cancellationToken, elementId, id);
    }

    public async ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("removeConnectionNodeId", cancellationToken, elementId, id);
    }

    public async ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        return await module.InvokeAsync<string?>("getModuleFromNodeId", cancellationToken, elementId, id);
    }

    public async ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("clearModuleSelected", cancellationToken, elementId);
    }

    public async ValueTask Clear(string elementId, CancellationToken cancellationToken = default)
    {
        var module = await Module();
        await module.InvokeVoidAsync("clear", cancellationToken, elementId);
    }

    private sealed class CallbackInvoker
    {
        private readonly EventCallback<string> _callback;
        public CallbackInvoker(EventCallback<string> callback) { _callback = callback; }
        [JSInvokable]
        public Task Invoke(string json) => _callback.InvokeAsync(json);
    }
}
