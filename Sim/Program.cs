namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        const int count = 2000;
        var results = new List<Simulation.Result>(count);
        for (var i = 0; i < count; i++)
        {
            var simulation = new Simulation();
            var result = simulation.Run();
            results.Add(result);
        }
        var purchaseAverageEquity = results.Average(x => x.PurchaseEquity);
        var rentAverageEquity = results.Average(x => x.RentEquity);
        Console.WriteLine($"Avg Purchase equity={purchaseAverageEquity:C}; Avg Rent equity={rentAverageEquity:C}");
    }
}