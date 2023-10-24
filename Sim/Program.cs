using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunSensitivityAnalysis();
    }

    private static void RunSensitivityAnalysis()
    {
        for (var i = 0d; i <= 0.2; i += 0.01)
        {
            var mortgageInterestRate = new InterestRate(Distributions.Constant(i + 0.02));
            var savingsInterestRate = new InterestRate(Distributions.Constant(i));
            var result = new Simulation().Run(
                mortgageInterestRate: mortgageInterestRate,
                savingsInterestRate: savingsInterestRate,
                rent: new RentPrice(700m, Distributions.Constant(0.03)),
                propertyValue: new PropertyValue(200_000, Distributions.Constant(0.03)),
                simulationYears: 5);
            Console.WriteLine($"Interest: {i:F2}; {result.ComputeDelta():C}");
        }
    }

    private static void RunStochasticSim()
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

    private static StreamWriter CreateConsoleStreamWriter()
    {
        var output = new StreamWriter(Console.OpenStandardOutput());
        output.AutoFlush = true;
        return output;
    }
}