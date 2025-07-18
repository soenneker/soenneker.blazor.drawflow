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
using System.Collections.Generic;

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
    private const string _interopName = "DrawflowInterop";

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

        await JsRuntime.InvokeVoidAsync($"{_interopName}.create", cancellationToken, elementId, json).NoSync();
    }

    public ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addNode", cancellationToken, elementId, name, inputs, outputs, posX, posY, className, data, html);
    }

    public ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeNode", cancellationToken, elementId, nodeId);
    }

    public ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addConnection", cancellationToken, elementId, outputNode, inputNode, outputClass, inputClass);
    }

    public ValueTask<string> Export(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>($"{_interopName}.export", cancellationToken, elementId);
    }

    public ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.import", cancellationToken, elementId, json);
    }

    public ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.destroy", cancellationToken, elementId);
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.createObserver", cancellationToken, elementId);
    }

    public ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default)
    {
        var dotNet = DotNetObjectReference.Create(new CallbackInvoker(callback));
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addEventListener", cancellationToken, elementId, eventName, dotNet);
    }

    public ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.zoomIn", cancellationToken, elementId);
    }

    public ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.zoomOut", cancellationToken, elementId);
    }

    public ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addModule", cancellationToken, elementId, name);
    }

    public ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.changeModule", cancellationToken, elementId, name);
    }

    public ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeModule", cancellationToken, elementId, name);
    }

    public ValueTask<IJSObjectReference?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<IJSObjectReference?>($"{_interopName}.getNodeFromId", cancellationToken, elementId, id);
    }

    public ValueTask<List<string>> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<string>>($"{_interopName}.getNodesFromName", cancellationToken, elementId, name);
    }

    public ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.updateNodeData", cancellationToken, elementId, id, data);
    }

    public ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addNodeInput", cancellationToken, elementId, id);
    }

    public ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addNodeOutput", cancellationToken, elementId, id);
    }

    public ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeNodeInput", cancellationToken, elementId, id, inputClass);
    }

    public ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeNodeOutput", cancellationToken, elementId, id, outputClass);
    }

    public ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeSingleConnection", cancellationToken, elementId, outId, inId, outClass, inClass);
    }

    public ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.updateConnectionNodes", cancellationToken, elementId, id);
    }

    public ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeConnectionNodeId", cancellationToken, elementId, id);
    }

    public ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string?>($"{_interopName}.getModuleFromNodeId", cancellationToken, elementId, id);
    }

    public ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.clearModuleSelected", cancellationToken, elementId);
    }

    public ValueTask Clear(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.clear", cancellationToken, elementId);
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
