using Microsoft.AspNetCore.Components;
using Soenneker.Blazor.Drawflow.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Blazor.Drawflow.Dtos;

namespace Soenneker.Blazor.Drawflow.Abstract;

/// <summary>
/// A Blazor interop library for drawflow.js
/// </summary>
public interface IDrawflowInterop : IAsyncDisposable
{
    /// <summary>
    /// Initialize the drawflow library with CDN or local resources
    /// </summary>
    /// <param name="useCdn">Whether to use CDN resources or local files</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the Drawflow is ready for use.</returns>
    ValueTask Initialize(bool useCdn, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a new drawflow instance
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="options">Configuration options for the drawflow instance</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the create operation is complete.</returns>
    ValueTask Create(string elementId, DrawflowOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Create a mutation observer for the drawflow element
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the observer creation is complete.</returns>
    ValueTask CreateObserver(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new node to the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="name">Name of the node</param>
    /// <param name="inputs">Number of inputs</param>
    /// <param name="outputs">Number of outputs</param>
    /// <param name="posX">X position</param>
    /// <param name="posY">Y position</param>
    /// <param name="className">CSS class name</param>
    /// <param name="data">Custom data object</param>
    /// <param name="html">HTML content for the node</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node addition is complete.</returns>
    ValueTask AddNode(string elementId, string name, int inputs, int outputs, int posX, int posY, string className, object? data, string html, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new node to the drawflow using strongly-typed DrawflowNode
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="node">Strongly-typed DrawflowNode object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node addition is complete.</returns>
    ValueTask AddNode(string elementId, DrawflowNode node, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a node from the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="nodeId">ID of the node to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node removal is complete.</returns>
    ValueTask RemoveNode(string elementId, string nodeId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a connection between two nodes
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="outputNode">Output node ID</param>
    /// <param name="inputNode">Input node ID</param>
    /// <param name="outputClass">Output class name</param>
    /// <param name="inputClass">Input class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the connection addition is complete.</returns>
    ValueTask AddConnection(string elementId, string outputNode, string inputNode, string outputClass, string inputClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Export the drawflow as JSON string
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>JSON string representation of the drawflow</returns>
    ValueTask<string> ExportAsJson(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Export the drawflow as strongly-typed object
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Strongly-typed DrawflowExport object</returns>
    ValueTask<DrawflowExport> Export(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Import drawflow data from JSON
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="json">JSON string to import</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the import operation is complete.</returns>
    ValueTask Import(string elementId, string json, CancellationToken cancellationToken = default);

    /// <summary>
    /// Import drawflow data from strongly-typed object
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="drawflowExport">Strongly-typed DrawflowExport object to import</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the import operation is complete.</returns>
    ValueTask Import(string elementId, DrawflowExport drawflowExport, CancellationToken cancellationToken = default);



    /// <summary>
    /// Destroy the drawflow instance
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the destroy operation is complete.</returns>
    ValueTask Destroy(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an event listener to the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="eventName">Name of the event to listen for</param>
    /// <param name="callback">Callback to execute when event occurs</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the event listener addition is complete.</returns>
    ValueTask AddEventListener(string elementId, string eventName, EventCallback<string> callback, CancellationToken cancellationToken = default);

    /// <summary>
    /// Zoom in the drawflow canvas
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the zoom in operation is complete.</returns>
    ValueTask ZoomIn(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Zoom out the drawflow canvas
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the zoom out operation is complete.</returns>
    ValueTask ZoomOut(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new module to the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="name">Name of the module</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the module addition is complete.</returns>
    ValueTask AddModule(string elementId, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add a new module to the drawflow using strongly-typed DrawflowModule
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="moduleName">Name of the module</param>
    /// <param name="module">Strongly-typed DrawflowModule object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the module addition is complete.</returns>
    ValueTask AddModule(string elementId, string moduleName, DrawflowModule module, CancellationToken cancellationToken = default);

    /// <summary>
    /// Change to a different module
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="name">Name of the module to change to</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the change module operation is complete.</returns>
    ValueTask ChangeModule(string elementId, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a module from the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="name">Name of the module to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the module removal is complete.</returns>
    ValueTask RemoveModule(string elementId, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a node by its ID
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Strongly-typed DrawflowNode object, or null if not found</returns>
    ValueTask<DrawflowNode?> GetNodeFromId(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all nodes with a specific name
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="name">Name to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of node IDs with the specified name</returns>
    ValueTask<List<string>> GetNodesFromName(string elementId, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the data of a specific node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="data">New data object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node data update is complete.</returns>
    ValueTask UpdateNodeData(string elementId, string id, object data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an input to a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node input addition is complete.</returns>
    ValueTask AddNodeInput(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an output to a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node output addition is complete.</returns>
    ValueTask AddNodeOutput(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an input from a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="inputClass">Input class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node input removal is complete.</returns>
    ValueTask RemoveNodeInput(string elementId, string id, string inputClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove an output from a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="outputClass">Output class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node output removal is complete.</returns>
    ValueTask RemoveNodeOutput(string elementId, string id, string outputClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a single connection between nodes
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="outId">Output node ID</param>
    /// <param name="inId">Input node ID</param>
    /// <param name="outClass">Output class name</param>
    /// <param name="inClass">Input class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the single connection removal is complete.</returns>
    ValueTask RemoveSingleConnection(string elementId, string outId, string inId, string outClass, string inClass, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update connection nodes after node changes
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the connection nodes update is complete.</returns>
    ValueTask UpdateConnectionNodes(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove all connections for a specific node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the connection node id removal is complete.</returns>
    ValueTask RemoveConnectionNodeId(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the module name for a specific node
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Module name</returns>
    ValueTask<string?> GetModuleFromNodeId(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear the currently selected module
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the Drawflow has been cleared.</returns>
    ValueTask ClearModuleSelected(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear all content from the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the Drawflow has been cleared.</returns>
    ValueTask Clear(string elementId, CancellationToken cancellationToken = default);

    // Additional methods from Drawflow documentation
    /// <summary>
    /// Set the zoom level of the drawflow
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="zoom">Zoom level (0.1 to 2.0)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the zoom has been stored.</returns>
    ValueTask SetZoom(string elementId, double zoom, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current zoom level
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current zoom level</returns>
    ValueTask<double> GetZoom(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Center the view on a specific node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the center node operation is complete.</returns>
    ValueTask CenterNode(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the position of a node
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Node position as object with x and y properties</returns>
    ValueTask<object> GetNodePosition(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the position of a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="posX">X position</param>
    /// <param name="posY">Y position</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node position has been stored.</returns>
    ValueTask SetNodePosition(string elementId, string id, int posX, int posY, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the HTML content of a node
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>HTML content string</returns>
    ValueTask<string> GetNodeHtml(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the HTML content of a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="html">HTML content</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node html has been stored.</returns>
    ValueTask SetNodeHtml(string elementId, string id, string html, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the CSS class of a node
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>CSS class name</returns>
    ValueTask<string> GetNodeClass(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the CSS class of a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="className">CSS class name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node class has been stored.</returns>
    ValueTask SetNodeClass(string elementId, string id, string className, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the name of a node
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Node name</returns>
    ValueTask<string> GetNodeName(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set the name of a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="name">Node name</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the node name has been stored.</returns>
    ValueTask SetNodeName(string elementId, string id, string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all connections for a node
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of connection objects</returns>
    ValueTask<List<object>> GetNodeConnections(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if a node is selected
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if node is selected</returns>
    ValueTask<bool> IsNodeSelected(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Select a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the select node operation is complete.</returns>
    ValueTask SelectNode(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Unselect a node
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="id">Node ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the unselect node operation is complete.</returns>
    ValueTask UnselectNode(string elementId, string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all selected nodes
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of selected node IDs</returns>
    ValueTask<List<string>> GetSelectedNodes(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clear all selected nodes
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the Drawflow has been cleared.</returns>
    ValueTask ClearSelectedNodes(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the current module name
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Current module name</returns>
    ValueTask<string> GetCurrentModule(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all module names
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of module names</returns>
    ValueTask<List<string>> GetModules(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if the drawflow is in edit mode
    /// </summary>
    /// <param name="elementId">The drawflow element ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if in edit mode</returns>
    ValueTask<bool> IsEditMode(string elementId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Set edit mode
    /// </summary>
    /// <param name="elementId">ID of the DOM element to target.</param>
    /// <param name="editMode">Whether edit mode.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task that completes when the edit mode has been stored.</returns>
    ValueTask SetEditMode(string elementId, bool editMode, CancellationToken cancellationToken = default);
}
