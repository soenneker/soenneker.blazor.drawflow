using Soenneker.Blazor.Drawflow.Abstract;
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
}
