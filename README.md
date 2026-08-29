[![NuGet](https://img.shields.io/nuget/v/soenneker.blazor.drawflow.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.drawflow/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.drawflow/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.drawflow/actions/workflows/codeql.yml)
[![Build Status](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.drawflow/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.drawflow/actions/workflows/publish-package.yml)
[![NuGet Downloads](https://img.shields.io/nuget/dt/soenneker.blazor.drawflow.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.drawflow/)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.drawflow)

# Soenneker.Blazor.Drawflow

A Blazor component and interop API for building editable, node-based diagrams with [Drawflow](https://github.com/jerosoler/Drawflow).

![Drawflow editor](https://github.com/user-attachments/assets/cda7f3b8-c440-4dcd-b035-54b2b03f0bef)

## Installation

```bash
dotnet add package Soenneker.Blazor.Drawflow
```

## Setup

Register the interop service in `Program.cs`:

```csharp
using Soenneker.Blazor.Drawflow.Registrars;

builder.Services.AddDrawflowInteropAsScoped();
```

Add the component namespace to `_Imports.razor`:

```razor
@using Soenneker.Blazor.Drawflow
@using Soenneker.Blazor.Drawflow.Options
```

## Create an editor

Give the component an explicit height; Drawflow needs a sized container to render a usable canvas.

```razor
<Drawflow @ref="_flow"
          Options="_options"
          OnNodeSelected="HandleSelection"
          style="height: 32rem; width: 100%;" />

@code {
    private Drawflow? _flow;

    private readonly DrawflowOptions _options = new()
    {
        Reroute = true,
        ZoomMin = 0.4,
        ZoomMax = 1.8,
        UseUuid = true
    };

    private Task HandleSelection(List<string> nodeIds)
    {
        // nodeIds contains the current selection reported by Drawflow.
        return Task.CompletedTask;
    }
}
```

The editor is created after its first render. Call methods through the component reference from a user action or after the parent component has rendered—not during `OnInitialized{Async}`.

## Add nodes and connections

```csharp
await _flow!.AddNode(
    name: "source",
    inputs: 0,
    outputs: 1,
    posX: 80,
    posY: 120,
    className: "source-node",
    data: new { endpoint = "/orders" },
    html: "<strong>Orders</strong>");

await _flow.AddNode(
    name: "processor",
    inputs: 1,
    outputs: 0,
    posX: 360,
    posY: 120,
    className: "processor-node",
    data: null,
    html: "<strong>Process order</strong>");

string sourceId = (await _flow.GetNodesFromName("source")).Single();
string processorId = (await _flow.GetNodesFromName("processor")).Single();

await _flow.AddConnection(sourceId, processorId, "output_1", "input_1");
```

Node HTML is inserted into the page by Drawflow. Never pass unsanitized user content to `AddNode`, `SetNodeHtml`, or imported flow data.

## Save and restore a flow

```csharp
string json = await _flow!.ExportAsJson();
await _flow.Import(json);
```

Use `Export()` and `Import(DrawflowExport)` when you prefer the models from `Soenneker.Blazor.Drawflow.Dtos`. Treat imported JSON as untrusted input: validate its size and contents before rendering it.

## Events

The component exposes callbacks for node, connection, module, selection, data, zoom, reroute, translation, import, and export events. `OnNodeSelected` and `OnNodeUnselected` provide `List<string>` values. The other callbacks expose Drawflow's JavaScript arguments as a JSON array in a `string`, so deserialize the payload according to the event you subscribe to.

## Asset loading

Drawflow's pinned JavaScript and CSS are loaded from jsDelivr by default with integrity checks. Set `UseCdn = false` to use the copies packaged with the NuGet package. Use the same setting for every Drawflow component in a scoped session because the shared assets are initialized once.
