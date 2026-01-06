using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Blazor.Drawflow.Options;
using Soenneker.Blazor.Drawflow.Utils;
using Soenneker.Blazor.Utils.EventListeningInterop;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Asyncs.Initializers;
using Soenneker.Utils.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Drawflow.Dtos;
using Soenneker.Extensions.Enumerable;

namespace Soenneker.Blazor.Drawflow;

/// <inheritdoc cref="IDrawflowInterop"/>
public sealed class DrawflowInterop : EventListeningInterop, IDrawflowInterop
{
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncInitializer _interopInitializer;
    private readonly AsyncInitializer<bool> _styleInitializer;
    private readonly AsyncInitializer<bool> _scriptInitializer;

    private const string _module = "Soenneker.Blazor.Drawflow/js/drawflowinterop.js";
    private const string _interopName = "DrawflowInterop";

    public DrawflowInterop(IJSRuntime jSRuntime, IResourceLoader resourceLoader) : base(jSRuntime)
    {
        _resourceLoader = resourceLoader;
        _styleInitializer = new AsyncInitializer<bool>(InitializeStyle);
        _scriptInitializer = new AsyncInitializer<bool>(InitializeScript);
        _interopInitializer = new AsyncInitializer(InitializeInterop);
    }

    private async ValueTask InitializeStyle(bool useCdn, CancellationToken token)
    {
        (string uri, string? integrity) style = DrawflowUtil.GetUriAndIntegrityForStyle(useCdn);
        await _resourceLoader.LoadStyle(style.uri, style.integrity, cancellationToken: token);
    }

    private async ValueTask InitializeScript(bool useCdn, CancellationToken token)
    {
        (string uri, string? integrity) script = DrawflowUtil.GetUriAndIntegrityForScript(useCdn);
        await _resourceLoader.LoadScriptAndWaitForVariable(script.uri, "Drawflow", script.integrity, cancellationToken: token);
    }

    private async ValueTask InitializeInterop(CancellationToken token)
    {
        await _resourceLoader.ImportModuleAndWaitUntilAvailable(_module, nameof(DrawflowInterop), 100, token);
    }

    public async ValueTask Initialize(bool useCdn, CancellationToken cancellationToken = default)
    {
        await _styleInitializer.Init(useCdn, cancellationToken);
        await _scriptInitializer.Init(useCdn, cancellationToken);
        await _interopInitializer.Init(cancellationToken);
    }

    public async ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default)
    {
        bool useCdn = options?.UseCdn ?? true;

        await _styleInitializer.Init(useCdn, cancellationToken);
        await _scriptInitializer.Init(useCdn, cancellationToken);
        await _interopInitializer.Init(cancellationToken);

        string? json = null;

        if (options != null)
            json = JsonUtil.Serialize(options);

        await JsRuntime.InvokeVoidAsync("DrawflowInterop.create", cancellationToken, elementId, json);
    }

    public ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.addNode", cancellationToken, elementId, name, inputs, outputs, posX, posY, className, data, html);
    }

    public async ValueTask AddNode(string elementId, DrawflowNode node, CancellationToken cancellationToken = default)
    {
        if (node.Name.IsNullOrEmpty())
            throw new ArgumentException("Node name cannot be null or empty", nameof(node));

        int inputs = node.Inputs?.Count ?? 0;
        int outputs = node.Outputs?.Count ?? 0;

        await JsRuntime.InvokeVoidAsync("DrawflowInterop.addNode", cancellationToken, elementId, node.Name, inputs, outputs, node.PosX, node.PosY, node.Class ?? "", node.Data, node.Html ?? "");
    }

    public ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.removeNode", cancellationToken, elementId, nodeId);
    }

    public ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.addConnection", cancellationToken, elementId, outputNode, inputNode, outputClass, inputClass);
    }

    public ValueTask<string> ExportAsJson(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>("DrawflowInterop.exportAsJson", cancellationToken, elementId);
    }

    public async ValueTask<DrawflowExport> Export(string elementId, CancellationToken cancellationToken = default)
    {
        string json = await JsRuntime.InvokeAsync<string>("DrawflowInterop.export", cancellationToken, elementId);
        return JsonUtil.Deserialize<DrawflowExport>(json) ?? new DrawflowExport();
    }

    public ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.import", cancellationToken, elementId, json);
    }

    public async ValueTask Import(string elementId, DrawflowExport drawflowExport, CancellationToken cancellationToken = default)
    {
        string? json = JsonUtil.Serialize(drawflowExport);
        await JsRuntime.InvokeVoidAsync("DrawflowInterop.import", cancellationToken, elementId, json);
    }

    public ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.destroy", cancellationToken, elementId);
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.createObserver", cancellationToken, elementId);
    }

    public ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default)
    {
        var dotNet = DotNetObjectReference.Create(new CallbackInvoker(callback));
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.addEventListener", cancellationToken, elementId, eventName, dotNet);
    }

    public ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.zoomIn", cancellationToken, elementId);
    }

    public ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.zoomOut", cancellationToken, elementId);
    }

    public ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.addModule", cancellationToken, elementId, name);
    }

    public async ValueTask AddModule(string elementId, string moduleName, DrawflowModule module, CancellationToken cancellationToken = default)
    {
        if (moduleName.IsNullOrEmpty())
            throw new ArgumentException("Module name cannot be null or empty", nameof(moduleName));

        // First add the module
        await JsRuntime.InvokeVoidAsync("DrawflowInterop.addModule", cancellationToken, elementId, moduleName);

        // Then add all nodes in the module
        if (module.Data != null)
        {
            foreach (DrawflowNode node in module.Data.Values)
            {
                await AddNode(elementId, node, cancellationToken);
            }
        }
    }

    public ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.changeModule", cancellationToken, elementId, name);
    }

    public ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.removeModule", cancellationToken, elementId, name);
    }

    public async ValueTask<DrawflowNode?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        string json = await JsRuntime.InvokeAsync<string>("DrawflowInterop.getNodeFromId", cancellationToken, elementId, id);
        if (string.IsNullOrWhiteSpace(json))
            return null;
        return JsonUtil.Deserialize<DrawflowNode>(json);
    }

    public ValueTask<List<string>> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<string>>("DrawflowInterop.getNodesFromName", cancellationToken, elementId, name);
    }

    public ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.updateNodeData", cancellationToken, elementId, id, data);
    }

    public ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.addNodeInput", cancellationToken, elementId, id);
    }

    public ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.addNodeOutput", cancellationToken, elementId, id);
    }

    public ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.removeNodeInput", cancellationToken, elementId, id, inputClass);
    }

    public ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.removeNodeOutput", cancellationToken, elementId, id, outputClass);
    }

    public ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.removeSingleConnection", cancellationToken, elementId, outId, inId, outClass, inClass);
    }

    public ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.updateConnectionNodes", cancellationToken, elementId, id);
    }

    public ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.removeConnectionNodeId", cancellationToken, elementId, id);
    }

    public ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string?>("DrawflowInterop.getModuleFromNodeId", cancellationToken, elementId, id);
    }

    public ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.clearModuleSelected", cancellationToken, elementId);
    }

    public ValueTask Clear(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.clear", cancellationToken, elementId);
    }

    public ValueTask SetZoom(string elementId, double zoom, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.setZoom", cancellationToken, elementId, zoom);
    }

    public ValueTask<double> GetZoom(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<double>("DrawflowInterop.getZoom", cancellationToken, elementId);
    }

    public ValueTask CenterNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.centerNode", cancellationToken, elementId, id);
    }

    public ValueTask<object> GetNodePosition(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<object>("DrawflowInterop.getNodePosition", cancellationToken, elementId, id);
    }

    public ValueTask SetNodePosition(string elementId, string id, int posX, int posY, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.setNodePosition", cancellationToken, elementId, id, posX, posY);
    }

    public ValueTask<string> GetNodeHtml(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>("DrawflowInterop.getNodeHtml", cancellationToken, elementId, id);
    }

    public ValueTask SetNodeHtml(string elementId, string id, string html, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.setNodeHtml", cancellationToken, elementId, id, html);
    }

    public ValueTask<string> GetNodeClass(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>("DrawflowInterop.getNodeClass", cancellationToken, elementId, id);
    }

    public ValueTask SetNodeClass(string elementId, string id, string className, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.setNodeClass", cancellationToken, elementId, id, className);
    }

    public ValueTask<string> GetNodeName(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>("DrawflowInterop.getNodeName", cancellationToken, elementId, id);
    }

    public ValueTask SetNodeName(string elementId, string id, string name, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.setNodeName", cancellationToken, elementId, id, name);
    }

    public ValueTask<List<object>> GetNodeConnections(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<object>>("DrawflowInterop.getNodeConnections", cancellationToken, elementId, id);
    }

    public ValueTask<bool> IsNodeSelected(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<bool>("DrawflowInterop.isNodeSelected", cancellationToken, elementId, id);
    }

    public ValueTask SelectNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.selectNode", cancellationToken, elementId, id);
    }

    public ValueTask UnselectNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.unselectNode", cancellationToken, elementId, id);
    }

    public ValueTask<List<string>> GetSelectedNodes(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<string>>("DrawflowInterop.getSelectedNodes", cancellationToken, elementId);
    }

    public ValueTask ClearSelectedNodes(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.clearSelectedNodes", cancellationToken, elementId);
    }

    public ValueTask<string> GetCurrentModule(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<string>("DrawflowInterop.getCurrentModule", cancellationToken, elementId);
    }

    public ValueTask<List<string>> GetModules(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<List<string>>("DrawflowInterop.getModules", cancellationToken, elementId);
    }

    public ValueTask<bool> IsEditMode(string elementId, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeAsync<bool>("DrawflowInterop.isEditMode", cancellationToken, elementId);
    }

    public ValueTask SetEditMode(string elementId, bool editMode, CancellationToken cancellationToken = default)
    {
        return JsRuntime.InvokeVoidAsync("DrawflowInterop.setEditMode", cancellationToken, elementId, editMode);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module);
        await _interopInitializer.DisposeAsync();
        await _styleInitializer.DisposeAsync();
        await _scriptInitializer.DisposeAsync();
    }

    private sealed class CallbackInvoker
    {
        private readonly EventCallback<string> _callback;
        public CallbackInvoker(EventCallback<string> callback) { _callback = callback; }
        [JSInvokable]
        public Task Invoke(string json) => _callback.InvokeAsync(json);
    }
}
