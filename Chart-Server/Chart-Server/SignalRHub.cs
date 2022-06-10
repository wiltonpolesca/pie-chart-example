using Microsoft.AspNetCore.SignalR;

namespace Chart.SignalR.Server;
public class SignalRHub : Hub
{
}

public class SignalRMessage
{
    private readonly IHubContext<SignalRHub> _hub;

    public SignalRMessage(IHubContext<SignalRHub> hub)
    {
        _hub = hub;
    }

    public async Task NotifyAllAsync(string method, object value, CancellationToken cancellationToken)
    {
        await _hub.Clients.All.SendAsync(method, value, cancellationToken);
    }
}

