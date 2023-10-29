using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunBaseline();
        RunOatSensitivityAnalyses();
        Run2DSensitivityAnalyses();
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

    private static void RunOatSensitivityAnalyses()
    {
        RunOatSensitivityAnalysis(
            "propertyValueYearlyIncrease",
            new Range(-0.15m, 0.15m, 0.01m),
            x => Simulation.Run(propertyValue: Baseline.PropertyValue with { YearlyIncrease = x }));
        RunOatSensitivityAnalysis(
            "mortgageInterestRate",
            new Range(-0m, 0.2m, 0.01m),
            x => Simulation.Run(mortgageInterestRate: new InterestRate(x)));
        RunOatSensitivityAnalysis(
            "savingsInterestRate",
            new Range(-0.1m, 0.2m, 0.01m),
            x => Simulation.Run(savingsInterestRate: new InterestRate(x)));
        RunOatSensitivityAnalysis(
            "initialMonthlyRentPrice",
            new Range(500m, 2000m, 100m),
            x => Simulation.Run(rent: Baseline.RentPrice with {InitialMonthly = x}));
    }

    private static void RunOatSensitivityAnalysis(string variable, Range range, Func<decimal, Simulation.Result> run)
    {
        using var writer = new CsvWriter($"{variable}.csv", variable, "delta");
        for (var i = range.Start; i <= range.Stop; i += range.Step)
        {
            var result = run(i).ComputeDelta();
            writer.WriteLine(i, result);
        }
    }

    private static void Run2DSensitivityAnalyses()
    {
        Run2DSensitivityAnalysis(
            "mortgageInterestRate", new Range(-0m, 0.2m, 0.01m),
            "savingsInterestRate", new Range(-0.1m, 0.2m, 0.01m),
            (x, y) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                savingsInterestRate: new InterestRate(y)));
        Run2DSensitivityAnalysis(
            "initialPropertyValue", new Range(100_000m, 500_000m, 10_000m),
            "initialMonthlyRentPrice", new Range(500m, 3000m, 100m),
            (x, y) => Simulation.Run(
                propertyValue: Baseline.PropertyValue with { InitialValue = x },
                rent: Baseline.RentPrice with { InitialMonthly = y }));
        Run2DSensitivityAnalysis(
            "mortgageInterestRate", new Range(-0m, 0.2m, 0.01m),
            "propertyValueYearlyIncrease", new Range(-0.15m, 0.15m, 0.01m),
            (x, y) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                propertyValue: Baseline.PropertyValue with {YearlyIncrease = y}));
    }

    private static void Run2DSensitivityAnalysis(
        string variable1,
        Range range1,
        string variable2,
        Range range2,
        Func<decimal, decimal, Simulation.Result> run)
    {
        using var writer = new CsvWriter($"{variable1}-{variable2}.csv", variable1, variable2, "delta");
        for (var i = range1.Start; i <= range1.Stop; i += range1.Step)
        {
            for (var j = range2.Start; j <= range2.Stop; j += range2.Step)
            {
                var result = run(i, j).ComputeDelta();
                writer.WriteLine(i, j, result);
            }
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