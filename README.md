[![](https://img.shields.io/nuget/v/soenneker.blazor.drawflow.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.drawflow/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.drawflow/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.drawflow/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.drawflow.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.drawflow/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.Drawflow
A lightweight Blazor wrapper for [drawflow.js](https://github.com/jerosoler/Drawflow).

## Installation
```bash
 dotnet add package Soenneker.Blazor.Drawflow
```

## Usage
Include the Drawflow library on the page and drop the component:
```html
<link href="https://cdn.jsdelivr.net/npm/drawflow/dist/drawflow.min.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/drawflow/dist/drawflow.min.js"></script>
```
```razor
<Drawflow @ref="Flow" Options="_options" OnNodeCreated="Handle" style="height:400px"></Drawflow>

@code {
    private Drawflow? Flow;
    private readonly DrawflowOptions _options = new();

    private Task Handle(string id)
    {
        Console.WriteLine($"Node created {id}");
        return Task.CompletedTask;
    }
}
```

See the demo project for a working example.

### Additional helpers

The component exposes many of the functions provided by drawflow.js. Some useful helpers include:

```razor
@ref Flow
```

```csharp
await Flow.ZoomIn();
await Flow.ZoomOut();
await Flow.AddModule("Other");
await Flow.ChangeModule("Other");
```

There are also methods for manipulating inputs/outputs, removing connections and clearing modules.
