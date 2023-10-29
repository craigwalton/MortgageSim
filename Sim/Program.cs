using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunBaseline();
        RunOatSensitivityAnalysis();
    }

    private static void RunBaseline()
    {
        using var writer = new CsvWriter(
            "baseline.csv",
            "simulationYears",
            "deposit",
            "initialPropertyValue",
            "propertyValueYearlyIncrease",
            "mortgageTerm",
            "mortgageInterestRate",
            "initialMonthlyRentPrice",
            "rentPriceYearlyIncrease",
            "savingsInterestRate",
            "delta"
        );
        var result = Simulation.Run();
        writer.WriteLine(
            Baseline.SimulationYears,
            Baseline.Deposit,
            Baseline.PropertyValue.InitialValue,
            Baseline.PropertyValue.YearlyIncrease,
            Baseline.MortgageTermYears,
            Baseline.MortgageInterestRate.Yearly,
            Baseline.RentPrice.InitialMonthly,
            Baseline.RentPrice.YearlyIncrease,
            Baseline.SavingsInterestRate.Yearly,
            result.ComputeDelta());
    }

    private static void RunOatSensitivityAnalysis()
    {
        RunOatSensitivityAnalysis("propertyValueYearlyIncrease", -0.15m, 0.15m, 0.01m,
            x => Simulation.Run(propertyValue: Baseline.PropertyValue with { YearlyIncrease = x }));
        RunOatSensitivityAnalysis("mortgageInterestRate", -0m, 0.2m, 0.01m,
            x => Simulation.Run(mortgageInterestRate: new InterestRate(x)));
        RunOatSensitivityAnalysis("savingsInterestRate", -0.1m, 0.2m, 0.01m,
            x => Simulation.Run(savingsInterestRate: new InterestRate(x)));
        RunOatSensitivityAnalysis("initialMonthlyRentPrice", 500m, 2000m, 100m,
            x => Simulation.Run(rent: Baseline.RentPrice with {InitialMonthly = x}));
    }

    private static void RunOatSensitivityAnalysis(
        string variable,
        decimal start,
        decimal stop,
        decimal step,
        Func<decimal, Simulation.Result> func)
    {
        using var writer = new CsvWriter($"{variable}.csv", variable, "delta");
        for (var i = start; i <= stop; i += step)
        {
            var result = func(i).ComputeDelta();
            writer.WriteLine(i, result);
        }
    }

    private static void Run2DSensitivityAnalysis()
    {
        using var writer = new CsvWriter("2d.csv", "interestRate", "initialRent", "delta");
        for (var i = -0.05m; i <= 0.2m; i += 0.01m)
        {
            for (var r = 500m; r <= 2000m; r += 100m)
            {
                var mortgageInterestRate = new InterestRate(i + 0.02m);
                var savingsInterestRate = new InterestRate(i);
                var result = Simulation.Run(mortgageInterestRate: mortgageInterestRate, rent: new RentPrice(r, Baseline.RentPrice.YearlyIncrease), savingsInterestRate: savingsInterestRate);
                writer.WriteLine(i, r, $"{result.ComputeDelta():F2}");
            }
        }
    }

    private static void Run3DSensitivityAnalysis()
    {
        using var writer = new CsvWriter("3d.csv", "interestRate", "propertyValueIncrease", "rentIncrease", "delta");
        for (var i = -0.05m; i <= 0.2m; i += 0.01m)
        {
            for (var p = -0.1m; p <= 0.1m; p += 0.01m)
            {
                for (var r = 0.0m; r <= 0.1m; r += 0.01m)
                {
                    var mortgageInterestRate = new InterestRate(i + 0.02m);
                    var savingsInterestRate = new InterestRate(i);
                    var result = Simulation.Run(propertyValue: Baseline.PropertyValue with {YearlyIncrease = p}, mortgageInterestRate: mortgageInterestRate, rent: Baseline.RentPrice with {YearlyIncrease = r}, savingsInterestRate: savingsInterestRate);
                    writer.WriteLine(i, p, r, $"{result.ComputeDelta():F2}");
                }
            }
        }
    }

    private static StreamWriter CreateConsoleStreamWriter()
    {
        var output = new StreamWriter(Console.OpenStandardOutput());
        output.AutoFlush = true;
        return output;
    }
}