using MathNet.Numerics.Distributions;

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
            var mortgageInterestRate = new InterestRate(new Normal(i + 0.02, 0.0));
            var savingsInterestRate = new InterestRate(new Normal(i, 0.0));
            var result = new Simulation().Run(
                mortgageInterestRate: mortgageInterestRate,
                savingsInterestRate: savingsInterestRate,
                rent: new RentPrice(700m, new Normal(0.03, 0)),
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