namespace Chart.SignalR.Server; 
public class PieChartData
{
    public IEnumerable<string> Labels { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<double> Values { get; set; } = Enumerable.Empty<Double>();
}
