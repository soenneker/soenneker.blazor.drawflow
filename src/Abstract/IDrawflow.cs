using Soenneker.Blazor.Drawflow.Options;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Drawflow.Dtos;
using Soenneker.Quark.Components.Cancellable.Abstract;

namespace Soenneker.Blazor.Drawflow.Abstract;

/// <summary>
/// Interface for the Drawflow Blazor component
/// </summary>
public interface IDrawflow : ICancellableComponent
{
    /// <summary>
    /// Create a new drawflow instance
    /// </summary>
    /// <param name="options">Configuration options for the drawflow instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask Create(DrawflowOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new node to the drawflow
    /// </summary>
    /// <param name="name">Name of the node</param>
    /// <param name="inputs">Number of inputs</param>
    /// <param name="outputs">Number of outputs</param>
    /// <param name="posX">X position</param>
    /// <param name="posY">Y position</param>
    /// <param name="className">CSS class name</param>
    /// <param name="data">Custom data object</param>
    /// <param name="html">HTML content for the node</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddNode(string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new node to the drawflow using strongly-typed DrawflowNode
    /// </summary>
    /// <param name="node">Strongly-typed DrawflowNode object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddNode(DrawflowNode node, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a connection between two nodes
    /// </summary>
    /// <param name="outputNode">Output node ID</param>
    /// <param name="inputNode">Input node ID</param>
    /// <param name="outputClass">Output class name</param>
    /// <param name="inputClass">Input class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddConnection(string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a node from the drawflow
    /// </summary>
    /// <param name="nodeId">ID of the node to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask RemoveNode(string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove all connections for a specific node
    /// </summary>
    /// <param name="nodeId">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask RemoveConnectionNodeId(string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a node by its ID
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Strongly-typed DrawflowNode object, or null if not found</returns>
    ValueTask<DrawflowNode?> GetNodeFromId(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all nodes with a specific name
    /// </summary>
    /// <param name="name">Name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of node IDs with the specified name</returns>
    ValueTask<List<string>> GetNodesFromName(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the data of a specific node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="data">New data object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask UpdateNodeData(string id, object data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update connection nodes after node changes
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask UpdateConnectionNodes(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the module name for a specific node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Module name</returns>
    ValueTask<string?> GetModuleFromNodeId(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear the currently selected module
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask ClearModuleSelected(CancellationToken cancellationToken = default);

    /// <summary>
    /// Export the drawflow as JSON string
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>JSON string representation of the drawflow</returns>
    ValueTask<string> ExportAsJson(CancellationToken cancellationToken = default);

    /// <summary>
    /// Export the drawflow as strongly-typed object
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Strongly-typed DrawflowExport object</returns>
    ValueTask<DrawflowExport> Export(CancellationToken cancellationToken = default);

    /// <summary>
    /// Import drawflow data from JSON
    /// </summary>
    /// <param name="json">JSON string to import</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask Import(string json, CancellationToken cancellationToken = default);

    /// <summary>
    /// Import drawflow data from strongly-typed object
    /// </summary>
    /// <param name="drawflowExport">Strongly-typed DrawflowExport object to import</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask Import(DrawflowExport drawflowExport, CancellationToken cancellationToken = default);

    /// <summary>
    /// Zoom in the drawflow canvas
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask ZoomIn(CancellationToken cancellationToken = default);

    /// <summary>
    /// Zoom out the drawflow canvas
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask ZoomOut(CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new module to the drawflow
    /// </summary>
    /// <param name="name">Name of the module</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddModule(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new module to the drawflow using strongly-typed DrawflowModule
    /// </summary>
    /// <param name="moduleName">Name of the module</param>
    /// <param name="module">Strongly-typed DrawflowModule object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddModule(string moduleName, DrawflowModule module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Change to a different module
    /// </summary>
    /// <param name="name">Name of the module to change to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask ChangeModule(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a module from the drawflow
    /// </summary>
    /// <param name="name">Name of the module to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask RemoveModule(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an input to a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddNodeInput(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an output to a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask AddNodeOutput(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an input from a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="inputClass">Input class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask RemoveNodeInput(string id, string inputClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an output from a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="outputClass">Output class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask RemoveNodeOutput(string id, string outputClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a single connection between nodes
    /// </summary>
    /// <param name="outId">Output node ID</param>
    /// <param name="inId">Input node ID</param>
    /// <param name="outClass">Output class name</param>
    /// <param name="inClass">Input class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask RemoveSingleConnection(string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear all content from the drawflow
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask Clear(CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the zoom level of the drawflow
    /// </summary>
    /// <param name="zoom">Zoom level (0.1 to 2.0)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SetZoom(double zoom, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current zoom level
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current zoom level</returns>
    ValueTask<double> GetZoom(CancellationToken cancellationToken = default);

    /// <summary>
    /// Center the view on a specific node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask CenterNode(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the position of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Node position as object with x and y properties</returns>
    ValueTask<object> GetNodePosition(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the position of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="posX">X position</param>
    /// <param name="posY">Y position</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SetNodePosition(string id, int posX, int posY, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the HTML content of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>HTML content string</returns>
    ValueTask<string> GetNodeHtml(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the HTML content of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="html">HTML content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SetNodeHtml(string id, string html, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the CSS class of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSS class name</returns>
    ValueTask<string> GetNodeClass(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the CSS class of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="className">CSS class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SetNodeClass(string id, string className, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the name of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Node name</returns>
    ValueTask<string> GetNodeName(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the name of a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="name">Node name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SetNodeName(string id, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all connections for a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of connection objects</returns>
    ValueTask<List<object>> GetNodeConnections(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a node is selected
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if node is selected</returns>
    ValueTask<bool> IsNodeSelected(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Select a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SelectNode(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unselect a node
    /// </summary>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask UnselectNode(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all selected nodes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of selected node IDs</returns>
    ValueTask<List<string>> GetSelectedNodes(CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear all selected nodes
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask ClearSelectedNodes(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current module name
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current module name</returns>
    ValueTask<string> GetCurrentModule(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all module names
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of module names</returns>
    ValueTask<List<string>> GetModules(CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if the drawflow is in edit mode
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if in edit mode</returns>
    ValueTask<bool> IsEditMode(CancellationToken cancellationToken = default);

    /// <summary>
    /// Set edit mode
    /// </summary>
    /// <param name="editMode">Whether to enable edit mode</param>
    /// <param name="cancellationToken">Cancellation token</param>
    ValueTask SetEditMode(bool editMode, CancellationToken cancellationToken = default);
}
