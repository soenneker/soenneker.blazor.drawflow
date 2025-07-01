[![NuGet](https://img.shields.io/nuget/v/soenneker.blazor.drawflow.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.drawflow/)
[![Build Status](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.drawflow/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.drawflow/actions/workflows/publish-package.yml)
[![NuGet Downloads](https://img.shields.io/nuget/dt/soenneker.blazor.drawflow.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.drawflow/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Drawflow

**Soenneker.Blazor.Drawflow** is a lightweight, modern Blazor wrapper for [drawflow.js](https://github.com/jerosoler/Drawflow), enabling interactive node-based diagrams in your Blazor applications. It supports advanced features like modules, zoom, import/export, and full event handling.

[Demo](https://soenneker.github.io/soenneker.blazor.drawflow)


## Features

- **Node and Connection Management**: Add, remove, and update nodes and connections programmatically.
- **Modules**: Organize nodes into modules and switch between them.
- **Zoom & Pan**: Built-in zoom controls and canvas panning.
- **Import/Export**: Serialize and restore flows as JSON.
- **Event Handling**: Subscribe to all major events (node created, removed, selected, data changed, etc).
- **Customizable**: Pass options to control rerouting, curvature, zoom limits, and more.
- **CDN or Local Assets**: Load drawflow.js and CSS from CDN or local files.

---

## Installation

```bash
dotnet add package Soenneker.Blazor.Drawflow
```

---

## Quick Start

1. **Register Services** (in `Program.cs`):

```csharp
builder.Services.AddDrawflowInteropAsScoped();
```

2. **Add the Component** (in your `.razor` file):

```razor
<Drawflow @ref="Flow" Options="_options" OnNodeCreated="HandleNodeCreated" style="height:400px"></Drawflow>

@code {
    private Drawflow? Flow;
    private readonly DrawflowOptions _options = new();

    private Task HandleNodeCreated(string id)
    {
        Console.WriteLine($"Node created {id}");
        return Task.CompletedTask;
    }
}
```

---

### Event Callbacks

```razor
<Drawflow
    @ref="Flow"
    Options="_options"
    OnNodeCreated="HandleNodeCreated"
    OnNodeRemoved="HandleNodeRemoved"
    OnConnectionCreated="HandleConnectionCreated"
    OnDataChanged="HandleDataChanged"
    ... />
```

### Programmatic API

```csharp
await Flow.AddNode("github", 1, 1, 150, 150, "github", new { name = "GitHub" }, "<div>GitHub</div>");
await Flow.AddConnection("github", "slack", "output", "input");
await Flow.ZoomIn();
await Flow.Export();
```

### Options

```csharp
var options = new DrawflowOptions {
    Reroute = true,
    Curvature = 0.3,
    Zoom = 1.0,
    ZoomMax = 2.0,
    ZoomMin = 0.3,
    DraggableInputs = true,
    UseUuid = true,
    ManualCreate = false // auto-create on render
};
```