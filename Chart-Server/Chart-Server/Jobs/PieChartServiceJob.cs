using Quartz;

namespace Chart.SignalR.Server;
public class PieChartServiceJob : IJob
{
    private readonly SignalRMessage _signalRMessage;

    public PieChartServiceJob(SignalRMessage signalRMessage)
    {
        _signalRMessage = signalRMessage;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var random = new Random();
        var data = new PieChartData()
        {
            Labels = new List<string> { "value 1", "value 2", "Value 3" },
            Values = new List<double> { random.Next(1, 500), random.Next(1, 500), random.Next(1, 500) },
        };

        await _signalRMessage.NotifyAllAsync("Pie", data, default);
    }
}
