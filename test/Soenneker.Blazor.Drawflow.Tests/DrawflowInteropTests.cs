using Soenneker.Blazor.Drawflow.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.Drawflow.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class DrawflowInteropTests : HostedUnitTest
{
    private readonly IDrawflowInterop _blazorlibrary;

    public DrawflowInteropTests(Host host) : base(host)
    {
        _blazorlibrary = Resolve<IDrawflowInterop>(true);
    }

    [Test]
    public void Default()
    {

    }
}
