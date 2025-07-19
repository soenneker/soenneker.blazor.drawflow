using System.Collections.Generic;
using System.Threading.Tasks;
using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Blazor.Drawflow.Models;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.Drawflow.Tests;

[Collection("Collection")]
public sealed class DrawflowInteropTests : FixturedUnitTest
{
    private readonly IDrawflowInterop _blazorlibrary;

    public DrawflowInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _blazorlibrary = Resolve<IDrawflowInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }

    [Fact]
    public async Task Export_ShouldReturnDrawflowGraph()
    {
        // Arrange
        string elementId = "test-element";

        // Act
        DrawflowExport result = await _blazorlibrary.Export(elementId);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Drawflow);
    }

    [Fact]
    public async Task ExportAsJson_ShouldReturnString()
    {
        // Arrange
        string elementId = "test-element";

        // Act
        string result = await _blazorlibrary.ExportAsJson(elementId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
    }



    [Fact]
    public async Task Import_WithDrawflowExport_ShouldNotThrow()
    {
        // Arrange
        string elementId = "test-element";
        var drawflowExport = new DrawflowExport
        {
            Drawflow = new Dictionary<string, DrawflowModule>
            {
                ["Home"] = new DrawflowModule
                {
                    Data = new Dictionary<string, DrawflowNode>()
                }
            }
        };

        // Act & Assert
        await _blazorlibrary.Import(elementId, drawflowExport);
    }

    [Fact]
    public async Task AddNode_WithDrawflowNode_ShouldNotThrow()
    {
        // Arrange
        string elementId = "test-element";
        var node = new DrawflowNode
        {
            Name = "TestNode",
            PosX = 100,
            PosY = 100,
            Class = "test-class",
            Html = "<div>Test</div>",
            Data = new Dictionary<string, object> { ["key"] = "value" }
        };

        // Act & Assert
        await _blazorlibrary.AddNode(elementId, node);
    }

    [Fact]
    public async Task AddModule_WithDrawflowModule_ShouldNotThrow()
    {
        // Arrange
        string elementId = "test-element";
        string moduleName = "TestModule";
        var module = new DrawflowModule
        {
            Data = new Dictionary<string, DrawflowNode>
            {
                ["node1"] = new DrawflowNode
                {
                    Name = "TestNode1",
                    PosX = 100,
                    PosY = 100,
                    Class = "test-class",
                    Html = "<div>Test1</div>"
                }
            }
        };

        // Act & Assert
        await _blazorlibrary.AddModule(elementId, moduleName, module);
    }
}
