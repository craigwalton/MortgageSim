using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        Run2DSensitivityAnalysis();
    }

    private static void RunSensitivityAnalysis()
    {
        for (var i = 0m; i <= 0.2m; i += 0.01m)
        {
            var mortgageInterestRate = new InterestRate(Distributions.Constant(i + 0.02m));
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

    private static void Run2DSensitivityAnalysis()
    {
        using var sw = new StreamWriter("../../../../data/2d.csv");
        sw.WriteLine("interestRate,propertyValueIncrease,delta");
        for (var i = -0.05m; i <= 0.2m; i += 0.01m)
        {
            for (var p = -0.1m; p <= 0.1m; p += 0.01m)
            {
                var mortgageInterestRate = new InterestRate(Distributions.Constant(i + 0.02m));
                var savingsInterestRate = new InterestRate(Distributions.Constant(i));
                var result = new Simulation().Run(
                    mortgageInterestRate: mortgageInterestRate,
                    savingsInterestRate: savingsInterestRate,
                    rent: new RentPrice(700m, Distributions.Constant(0.03)),
                    propertyValue: new PropertyValue(200_000, Distributions.Constant(p)),
                    simulationYears: 5);
                sw.WriteLine($"{i},{p},{result.ComputeDelta():F2}");
            }
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