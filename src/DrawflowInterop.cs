using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Blazor.Drawflow.Options;
using Soenneker.Blazor.Drawflow.Utils;
using Soenneker.Blazor.Utils.EventListeningInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;
using Soenneker.Utils.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Drawflow.Dtos;

namespace Soenneker.Blazor.Drawflow;

/// <inheritdoc cref="IDrawflowInterop"/>
public sealed class DrawflowInterop : EventListeningInterop, IDrawflowInterop
{
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton _interopInitializer;
    private readonly AsyncSingleton _styleInitializer;
    private readonly AsyncSingleton _scriptInitializer;

    private const string _module = "Soenneker.Blazor.Drawflow/js/drawflowinterop.js";
    private const string _interopName = "DrawflowInterop";

    public DrawflowInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader) : base(jSRuntime)
    {
        _resourceLoader = resourceLoader;

        _styleInitializer = new AsyncSingleton(async (token, obj) =>
        {
            var useCdn = true;

            if (obj.Length > 0)
                useCdn = (bool)obj[0];

            (string uri, string? integrity) style = DrawflowUtil.GetUriAndIntegrityForStyle(useCdn);

            await _resourceLoader.LoadStyle(style.uri, style.integrity, cancellationToken: token).NoSync();
            return new object();
        });

        _scriptInitializer = new AsyncSingleton(async (token, obj) =>
        {
            var useCdn = true;

            if (obj.Length > 0)
                useCdn = (bool)obj[0];

            (string uri, string? integrity) script = DrawflowUtil.GetUriAndIntegrityForScript(useCdn);

            await _resourceLoader.LoadScriptAndWaitForVariable(script.uri, "Drawflow", script.integrity, cancellationToken: token).NoSync();
            return new object();
        });

        _interopInitializer = new AsyncSingleton(async (token, _) =>
        {
            await resourceLoader.ImportModuleAndWaitUntilAvailable(_module, nameof(DrawflowInterop), 100, token).NoSync();
            return new object();
        });
    }

    public async ValueTask Initialize(bool useCdn, CancellationToken cancellationToken = default)
    {
        await _styleInitializer.Init(cancellationToken, useCdn).NoSync();
        await _scriptInitializer.Init(cancellationToken, useCdn).NoSync();
        await _interopInitializer.Init(cancellationToken).NoSync();
    }

    public async ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default)
    {
        bool useCdn = options?.UseCdn ?? true;

        await _styleInitializer.Init(cancellationToken, useCdn).NoSync();
        await _scriptInitializer.Init(cancellationToken, useCdn).NoSync();
        await _interopInitializer.Init(cancellationToken).NoSync();

        string? json = null;

        if (options != null)
            json = JsonUtil.Serialize(options);

        await JsRuntime.InvokeVoidAsync($"{_interopName}.create", cancellationToken, elementId, json).NoSync();
    }

    public ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addNode", cancellationToken, elementId, name, inputs, outputs, posX, posY, className, data, html);
    }

    public async ValueTask AddNode(string elementId, DrawflowNode node, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(node.Name))
            throw new ArgumentException("Node name cannot be null or empty", nameof(node));

        int inputs = node.Inputs?.Count ?? 0;
        int outputs = node.Outputs?.Count ?? 0;

        await JsRuntime.InvokeVoidAsync($"{_interopName}.addNode", cancellationToken, elementId, node.Name, inputs, outputs, node.PosX, node.PosY, node.Class ?? "", node.Data, node.Html ?? "").NoSync();
    }

    public ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeNode", cancellationToken, elementId, nodeId);
    }

    public ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.addConnection", cancellationToken, elementId, outputNode, inputNode, outputClass, inputClass);
    }

    public ValueTask<string> ExportAsJson(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>($"{_interopName}.exportAsJson", cancellationToken, elementId);
    }

    public async ValueTask<DrawflowExport> Export(string elementId, CancellationToken cancellationToken = default)
    {
        string json = await JsRuntime.InvokeAsync<string>($"{_interopName}.export", cancellationToken, elementId).NoSync();
        return JsonUtil.Deserialize<DrawflowExport>(json) ?? new DrawflowExport();
    }

    public ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.import", cancellationToken, elementId, json);
    }

    public async ValueTask Import(string elementId, DrawflowExport drawflowExport, CancellationToken cancellationToken = default)
    {
        string? json = JsonUtil.Serialize(drawflowExport);
        await JsRuntime.InvokeVoidAsync($"{_interopName}.import", cancellationToken, elementId, json).NoSync();
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

    public async ValueTask AddModule(string elementId, string moduleName, DrawflowModule module, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(moduleName))
            throw new ArgumentException("Module name cannot be null or empty", nameof(moduleName));

        // First add the module
        await JsRuntime.InvokeVoidAsync($"{_interopName}.addModule", cancellationToken, elementId, moduleName).NoSync();

        // Then add all nodes in the module
        if (module.Data != null)
        {
            foreach (DrawflowNode node in module.Data.Values)
            {
                await AddNode(elementId, node, cancellationToken).NoSync();
            }
        }
    }

    public ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.changeModule", cancellationToken, elementId, name);
    }

    public ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.removeModule", cancellationToken, elementId, name);
    }

    public async ValueTask<DrawflowNode?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        string json = await JsRuntime.InvokeAsync<string>($"{_interopName}.getNodeFromId", cancellationToken, elementId, id).NoSync();
        if (string.IsNullOrWhiteSpace(json))
            return null;
        return JsonUtil.Deserialize<DrawflowNode>(json);
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

    public ValueTask SetZoom(string elementId, double zoom, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.setZoom", cancellationToken, elementId, zoom);
    }

    public ValueTask<double> GetZoom(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<double>($"{_interopName}.getZoom", cancellationToken, elementId);
    }

    public ValueTask CenterNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.centerNode", cancellationToken, elementId, id);
    }

    public ValueTask<object> GetNodePosition(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<object>($"{_interopName}.getNodePosition", cancellationToken, elementId, id);
    }

    public ValueTask SetNodePosition(string elementId, string id, int posX, int posY, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.setNodePosition", cancellationToken, elementId, id, posX, posY);
    }

    public ValueTask<string> GetNodeHtml(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>($"{_interopName}.getNodeHtml", cancellationToken, elementId, id);
    }

    public ValueTask SetNodeHtml(string elementId, string id, string html, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.setNodeHtml", cancellationToken, elementId, id, html);
    }

    public ValueTask<string> GetNodeClass(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>($"{_interopName}.getNodeClass", cancellationToken, elementId, id);
    }

    public ValueTask SetNodeClass(string elementId, string id, string className, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.setNodeClass", cancellationToken, elementId, id, className);
    }

    public ValueTask<string> GetNodeName(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>($"{_interopName}.getNodeName", cancellationToken, elementId, id);
    }

    public ValueTask SetNodeName(string elementId, string id, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.setNodeName", cancellationToken, elementId, id, name);
    }

    public ValueTask<List<object>> GetNodeConnections(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<object>>($"{_interopName}.getNodeConnections", cancellationToken, elementId, id);
    }

    public ValueTask<bool> IsNodeSelected(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<bool>($"{_interopName}.isNodeSelected", cancellationToken, elementId, id);
    }

    public ValueTask SelectNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.selectNode", cancellationToken, elementId, id);
    }

    public ValueTask UnselectNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.unselectNode", cancellationToken, elementId, id);
    }

    public ValueTask<List<string>> GetSelectedNodes(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<string>>($"{_interopName}.getSelectedNodes", cancellationToken, elementId);
    }

    public ValueTask ClearSelectedNodes(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.clearSelectedNodes", cancellationToken, elementId);
    }

    public ValueTask<string> GetCurrentModule(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>($"{_interopName}.getCurrentModule", cancellationToken, elementId);
    }

    public ValueTask<List<string>> GetModules(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<string>>($"{_interopName}.getModules", cancellationToken, elementId);
    }

    public ValueTask<bool> IsEditMode(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<bool>($"{_interopName}.isEditMode", cancellationToken, elementId);
    }

    public ValueTask SetEditMode(string elementId, bool editMode, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync($"{_interopName}.setEditMode", cancellationToken, elementId, editMode);
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
