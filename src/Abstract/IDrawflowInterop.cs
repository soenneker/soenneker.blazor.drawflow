using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Soenneker.Blazor.Drawflow.Options;

namespace Soenneker.Blazor.Drawflow.Abstract;

/// <summary>
/// A Blazor interop library for drawflow.js
/// </summary>
public interface IDrawflowInterop
{
    ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default);
    ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default);
    ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default);
    ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default);
    ValueTask<string> Export(string elementId, CancellationToken cancellationToken = default);
    ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default);
    ValueTask Destroy(string elementId, CancellationToken cancellationToken = default);
    ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default);

    ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default);
    ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default);

    ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default);
    ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default);
    ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default);

    ValueTask<IJSObjectReference?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default);
    ValueTask<int[]> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default);
    ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default);
    ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default);
    ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default);
    ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default);
    ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default);
    ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default);
    ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default);
    ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default);
    ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default);
    ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default);
    ValueTask Clear(string elementId, CancellationToken cancellationToken = default);
}
