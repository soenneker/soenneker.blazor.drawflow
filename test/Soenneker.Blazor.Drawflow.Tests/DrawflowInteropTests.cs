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
}
