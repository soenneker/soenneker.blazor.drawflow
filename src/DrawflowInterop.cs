using Soenneker.Blazor.Drawflow.Abstract;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Soenneker.Blazor.Drawflow.Options;
using Soenneker.Utils.Json;
using System.Threading;
using Soenneker.Blazor.Utils.EventListeningInterop;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Blazor.Drawflow.Utils;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Blazor.Drawflow;

/// <inheritdoc cref="IDrawflowInterop"/>
public sealed class DrawflowInterop : EventListeningInterop, IDrawflowInterop
{
    private readonly ILogger<DrawflowInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton _interopInitializer;
    private readonly AsyncSingleton _styleInitializer;
    private readonly AsyncSingleton _scriptInitializer;

    private const string _module = "Soenneker.Blazor.Drawflow/js/drawflowinterop.js";

    public DrawflowInterop(IJSRuntime jSRuntime, ILogger<DrawflowInterop> logger, IResourceLoader resourceLoader) : base(jSRuntime)
    {
        _logger = logger;
        _resourceLoader = resourceLoader;

        _interopInitializer = new AsyncSingleton(async (token, _) =>
        {
            await resourceLoader.ImportModuleAndWaitUntilAvailable(_module, nameof(DrawflowInterop), 100, token).NoSync();
            return new object();
        });

        _styleInitializer = new AsyncSingleton(async (token, obj) =>
        {
            var useCdn = true;

            if (obj.Length > 0)
                useCdn = (bool) obj[0];

            (string? uri, string? integrity) style = DrawflowUtil.GetUriAndIntegrityForStyle(useCdn);

            await _resourceLoader.LoadStyle(style.uri, style.integrity, cancellationToken: token).NoSync();
            return new object();
        });

        _scriptInitializer = new AsyncSingleton(async (token, obj) =>
        {
            var useCdn = true;

            if (obj.Length > 0)
                useCdn = (bool) obj[0];

            (string? uri, string? integrity) script = DrawflowUtil.GetUriAndIntegrityForScript(useCdn);

            await _resourceLoader.LoadScriptAndWaitForVariable(script.uri, "Drawflow", script.integrity, cancellationToken: token).NoSync();
            return new object();
        });
    }

    public ValueTask Initialize(bool useCdn, CancellationToken cancellationToken = default)
    {
        return _interopInitializer.Init(cancellationToken, useCdn);
    }

    public async ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default)
    {
        bool useCdn = options?.UseCdn ?? true;

        await _interopInitializer.Init(cancellationToken, useCdn).NoSync();
        await _styleInitializer.Init(cancellationToken, useCdn).NoSync();
        await _scriptInitializer.Init(cancellationToken, useCdn).NoSync();

        string? json = null;

        if (options != null)
            json = JsonUtil.Serialize(options);

        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.create", cancellationToken, elementId, json).NoSync();
    }

    public async ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.addNode", cancellationToken, elementId, name, inputs, outputs, posX, posY, className, data, html).NoSync();
    }

    public async ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.removeNode", cancellationToken, elementId, nodeId).NoSync();
    }

    public async ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.addConnection", cancellationToken, elementId, outputNode, inputNode, outputClass, inputClass).NoSync();
    }

    public async ValueTask<string> Export(string elementId, CancellationToken cancellationToken = default)
    {
        return await JsRuntime.InvokeAsync<string>($"{nameof(DrawflowInterop)}.export", cancellationToken, elementId).NoSync();
    }

    public async ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.import", cancellationToken, elementId, json).NoSync();
    }

    public async ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.destroy", cancellationToken, elementId).NoSync();
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.createObserver", cancellationToken, elementId);
    }

    public async ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default)
    {
        var dotNet = DotNetObjectReference.Create(new CallbackInvoker(callback));
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.addEventListener", cancellationToken, elementId, eventName, dotNet).NoSync();
    }

    public async ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.zoomIn", cancellationToken, elementId).NoSync();
    }

    public async ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.zoomOut", cancellationToken, elementId).NoSync();
    }

    public async ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.addModule", cancellationToken, elementId, name).NoSync();
    }

    public async ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.changeModule", cancellationToken, elementId, name).NoSync();
    }

    public async ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.removeModule", cancellationToken, elementId, name).NoSync();
    }

    public async ValueTask<IJSObjectReference?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return await JsRuntime.InvokeAsync<IJSObjectReference?>($"{nameof(DrawflowInterop)}.getNodeFromId", cancellationToken, elementId, id).NoSync();
    }

    public async ValueTask<int[]> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return await JsRuntime.InvokeAsync<int[]>($"{nameof(DrawflowInterop)}.getNodesFromName", cancellationToken, elementId, name).NoSync();
    }

    public async ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.updateNodeData", cancellationToken, elementId, id, data).NoSync();
    }

    public async ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.addNodeInput", cancellationToken, elementId, id).NoSync();
    }

    public async ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.addNodeOutput", cancellationToken, elementId, id).NoSync();
    }

    public async ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.removeNodeInput", cancellationToken, elementId, id, inputClass).NoSync();
    }

    public async ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.removeNodeOutput", cancellationToken, elementId, id, outputClass).NoSync();
    }

    public async ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.removeSingleConnection", cancellationToken, elementId, outId, inId, outClass, inClass).NoSync();
    }

    public async ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.updateConnectionNodes", cancellationToken, elementId, id).NoSync();
    }

    public async ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.removeConnectionNodeId", cancellationToken, elementId, id).NoSync();
    }

    public async ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return await JsRuntime.InvokeAsync<string?>($"{nameof(DrawflowInterop)}.getModuleFromNodeId", cancellationToken, elementId, id).NoSync();
    }

    public async ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.clearModuleSelected", cancellationToken, elementId).NoSync();
    }

    public async ValueTask Clear(string elementId, CancellationToken cancellationToken = default)
    {
        await JsRuntime.InvokeVoidAsync($"{nameof(DrawflowInterop)}.clear", cancellationToken, elementId).NoSync();
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module).NoSync();
        await _interopInitializer.DisposeAsync().NoSync();
        await _styleInitializer.DisposeAsync().NoSync();
        await _scriptInitializer.DisposeAsync().NoSync();
    }

    private sealed class CallbackInvoker
    {
        private readonly EventCallback<string> _callback;
        public CallbackInvoker(EventCallback<string> callback) { _callback = callback; }
        [JSInvokable]
        public Task Invoke(string json) => _callback.InvokeAsync(json);
    }
}
