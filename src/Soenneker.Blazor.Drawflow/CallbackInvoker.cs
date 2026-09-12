using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Soenneker.Blazor.Drawflow;

internal sealed class CallbackInvoker
{
    private readonly EventCallback<string> _callback;
    public CallbackInvoker(EventCallback<string> callback) { _callback = callback; }
    [JSInvokable]
    public Task Invoke(string json) => _callback.InvokeAsync(json);
}
