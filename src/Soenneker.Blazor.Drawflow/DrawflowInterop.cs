using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Blazor.Drawflow.Dtos;
using Soenneker.Blazor.Drawflow.Options;
using Soenneker.Blazor.Drawflow.Utils;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Extensions.Enumerable;
using Soenneker.Utils.CancellationScopes;
using Soenneker.Utils.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Drawflow;

/// <inheritdoc cref="IDrawflowInterop"/>
public sealed class DrawflowInterop : IDrawflowInterop
{
    private const string _modulePath = "/_content/Soenneker.Blazor.Drawflow/js/drawflowinterop.js";

    private readonly IResourceLoader _resourceLoader;
    private readonly IModuleImportUtil _moduleImportUtil;

    private readonly AsyncInitializer _interopInitializer;
    private readonly AsyncInitializer<bool> _styleInitializer;
    private readonly AsyncInitializer<bool> _scriptInitializer;
    private const string _drawflowVariable = "Drawflow";

    private readonly CancellationScope _cancellationScope = new();

    public DrawflowInterop(IResourceLoader resourceLoader, IModuleImportUtil moduleImportUtil)
    {
        _resourceLoader = resourceLoader;
        _moduleImportUtil = moduleImportUtil;
        _styleInitializer = new AsyncInitializer<bool>(InitializeStyle);
        _scriptInitializer = new AsyncInitializer<bool>(InitializeScript);
        _interopInitializer = new AsyncInitializer(InitializeInterop);
    }

    private static string NormalizeContentUri(string uri)
    {
        if (string.IsNullOrEmpty(uri) || uri.Contains("://", StringComparison.Ordinal))
            return uri;

        return uri[0] == '/' ? uri : "/" + uri;
    }

    private async ValueTask InitializeStyle(bool useCdn, CancellationToken token)
    {
        (string uri, string? integrity) style = DrawflowUtil.GetUriAndIntegrityForStyle(useCdn);
        await _resourceLoader.LoadStyle(NormalizeContentUri(style.uri), style.integrity, cancellationToken: token);
    }

    private async ValueTask InitializeScript(bool useCdn, CancellationToken token)
    {
        (string uri, string? integrity) script = DrawflowUtil.GetUriAndIntegrityForScript(useCdn);
        await _resourceLoader.LoadScriptAndWaitForVariable(NormalizeContentUri(script.uri), _drawflowVariable, script.integrity, cancellationToken: token);
    }

    private async ValueTask InitializeInterop(CancellationToken token)
    {
        _ = await _moduleImportUtil.GetContentModuleReference(_modulePath, token);
    }

    private async ValueTask<IJSObjectReference> GetModule(CancellationToken cancellationToken)
    {
        return await _moduleImportUtil.GetContentModuleReference(_modulePath, cancellationToken);
    }

    private async ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[] args)
    {
        IJSObjectReference module = await GetModule(cancellationToken);
        await module.InvokeVoidAsync(identifier, cancellationToken, args);
    }

    private async ValueTask<T> InvokeAsync<T>(string identifier, CancellationToken cancellationToken, params object?[] args)
    {
        IJSObjectReference module = await GetModule(cancellationToken);
        return await module.InvokeAsync<T>(identifier, cancellationToken, args);
    }

    public async ValueTask Initialize(bool useCdn, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await _styleInitializer.Init(useCdn, linked);
            await _scriptInitializer.Init(useCdn, linked);
            await _interopInitializer.Init(linked);
        }
    }

    public async ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            bool useCdn = options?.UseCdn ?? true;

            await _styleInitializer.Init(useCdn, linked);
            await _scriptInitializer.Init(useCdn, linked);
            await _interopInitializer.Init(linked);

            string? json = null;

            if (options != null)
                json = JsonUtil.Serialize(options);

            await InvokeVoidAsync("create", linked, elementId, json);
        }
    }

    public ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("addNode", linked, elementId, name, inputs, outputs, posX, posY, className, data, html);
        }
    }

    public async ValueTask AddNode(string elementId, DrawflowNode node, CancellationToken cancellationToken = default)
    {
        if (node.Name.IsNullOrEmpty())
            throw new ArgumentException("Node name cannot be null or empty", nameof(node));

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            int inputs = node.Inputs?.Count ?? 0;
            int outputs = node.Outputs?.Count ?? 0;

            await InvokeVoidAsync("addNode", linked, elementId, node.Name, inputs, outputs, node.PosX, node.PosY, node.Class ?? "", node.Data, node.Html ?? "");
        }
    }

    public ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("removeNode", linked, elementId, nodeId);
        }
    }

    public ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("addConnection", linked, elementId, outputNode, inputNode, outputClass, inputClass);
        }
    }

    public ValueTask<string> ExportAsJson(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<string>("exportAsJson", linked, elementId);
        }
    }

    public async ValueTask<DrawflowExport> Export(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            string json = await InvokeAsync<string>("exportFlow", linked, elementId);
            return JsonUtil.Deserialize<DrawflowExport>(json) ?? new DrawflowExport();
        }
    }

    public ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("importFlow", linked, elementId, json);
        }
    }

    public async ValueTask Import(string elementId, DrawflowExport drawflowExport, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            string? json = JsonUtil.Serialize(drawflowExport);
            await InvokeVoidAsync("importFlow", linked, elementId, json);
        }
    }

    public ValueTask Destroy(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("destroy", linked, elementId);
        }
    }

    public ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("createObserver", linked, elementId);
        }
    }

    public ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            var dotNet = DotNetObjectReference.Create(new CallbackInvoker(callback));
            return InvokeVoidAsync("addEventListener", linked, elementId, eventName, dotNet);
        }
    }

    public ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("zoomIn", linked, elementId);
        }
    }

    public ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("zoomOut", linked, elementId);
        }
    }

    public ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("addModule", linked, elementId, name);
        }
    }

    public async ValueTask AddModule(string elementId, string moduleName, DrawflowModule module, CancellationToken cancellationToken = default)
    {
        if (moduleName.IsNullOrEmpty())
            throw new ArgumentException("Module name cannot be null or empty", nameof(moduleName));

        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            await InvokeVoidAsync("addModule", linked, elementId, moduleName);

            if (module.Data != null)
            {
                foreach (DrawflowNode node in module.Data.Values)
                    await AddNode(elementId, node, linked);
            }
        }
    }

    public ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("changeModule", linked, elementId, name);
        }
    }

    public ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("removeModule", linked, elementId, name);
        }
    }

    public async ValueTask<DrawflowNode?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            string json = await InvokeAsync<string>("getNodeFromId", linked, elementId, id);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonUtil.Deserialize<DrawflowNode>(json);
        }
    }

    public ValueTask<List<string>> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<List<string>>("getNodesFromName", linked, elementId, name);
        }
    }

    public ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("updateNodeData", linked, elementId, id, data);
        }
    }

    public ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("addNodeInput", linked, elementId, id);
        }
    }

    public ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("addNodeOutput", linked, elementId, id);
        }
    }

    public ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("removeNodeInput", linked, elementId, id, inputClass);
        }
    }

    public ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("removeNodeOutput", linked, elementId, id, outputClass);
        }
    }

    public ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("removeSingleConnection", linked, elementId, outId, inId, outClass, inClass);
        }
    }

    public ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("updateConnectionNodes", linked, elementId, id);
        }
    }

    public ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("removeConnectionNodeId", linked, elementId, id);
        }
    }

    public ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<string?>("getModuleFromNodeId", linked, elementId, id);
        }
    }

    public ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("clearModuleSelected", linked, elementId);
        }
    }

    public ValueTask Clear(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("clear", linked, elementId);
        }
    }

    public ValueTask SetZoom(string elementId, double zoom, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("setZoom", linked, elementId, zoom);
        }
    }

    public ValueTask<double> GetZoom(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<double>("getZoom", linked, elementId);
        }
    }

    public ValueTask CenterNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("centerNode", linked, elementId, id);
        }
    }

    public ValueTask<object> GetNodePosition(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<object>("getNodePosition", linked, elementId, id);
        }
    }

    public ValueTask SetNodePosition(string elementId, string id, int posX, int posY, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("setNodePosition", linked, elementId, id, posX, posY);
        }
    }

    public ValueTask<string> GetNodeHtml(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<string>("getNodeHtml", linked, elementId, id);
        }
    }

    public ValueTask SetNodeHtml(string elementId, string id, string html, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("setNodeHtml", linked, elementId, id, html);
        }
    }

    public ValueTask<string> GetNodeClass(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<string>("getNodeClass", linked, elementId, id);
        }
    }

    public ValueTask SetNodeClass(string elementId, string id, string className, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("setNodeClass", linked, elementId, id, className);
        }
    }

    public ValueTask<string> GetNodeName(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<string>("getNodeName", linked, elementId, id);
        }
    }

    public ValueTask SetNodeName(string elementId, string id, string name, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("setNodeName", linked, elementId, id, name);
        }
    }

    public ValueTask<List<object>> GetNodeConnections(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<List<object>>("getNodeConnections", linked, elementId, id);
        }
    }

    public ValueTask<bool> IsNodeSelected(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<bool>("isNodeSelected", linked, elementId, id);
        }
    }

    public ValueTask SelectNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("selectNode", linked, elementId, id);
        }
    }

    public ValueTask UnselectNode(string elementId, string id, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("unselectNode", linked, elementId, id);
        }
    }

    public ValueTask<List<string>> GetSelectedNodes(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<List<string>>("getSelectedNodes", linked, elementId);
        }
    }

    public ValueTask ClearSelectedNodes(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("clearSelectedNodes", linked, elementId);
        }
    }

    public ValueTask<string> GetCurrentModule(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<string>("getCurrentModule", linked, elementId);
        }
    }

    public ValueTask<List<string>> GetModules(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<List<string>>("getModules", linked, elementId);
        }
    }

    public ValueTask<bool> IsEditMode(string elementId, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeAsync<bool>("isEditMode", linked, elementId);
        }
    }

    public ValueTask SetEditMode(string elementId, bool editMode, CancellationToken cancellationToken = default)
    {
        CancellationToken linked = _cancellationScope.CancellationToken.Link(cancellationToken, out CancellationTokenSource? source);

        using (source)
        {
            return InvokeVoidAsync("setEditMode", linked, elementId, editMode);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_modulePath);

        await _interopInitializer.DisposeAsync();
        await _styleInitializer.DisposeAsync();
        await _scriptInitializer.DisposeAsync();

        await _cancellationScope.DisposeAsync();
    }

    private sealed class CallbackInvoker
    {
        private readonly EventCallback<string> _callback;
        public CallbackInvoker(EventCallback<string> callback) { _callback = callback; }
        [JSInvokable]
        public Task Invoke(string json) => _callback.InvokeAsync(json);
    }
}
