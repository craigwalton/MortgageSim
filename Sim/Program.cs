using PropertySim.Experiments;
using PropertySim.Variables;
using Range = PropertySim.Experiments.Range;

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
            Ranges.PropertyValueYearlyIncrease,
            x => Simulation.Run(propertyValue: Baseline.PropertyValue with { YearlyIncrease = x }));
        RunOatSensitivityAnalysis(
            "mortgageInterestRate",
            Ranges.MortgageInterestRateRange,
            x => Simulation.Run(mortgageInterestRate: new InterestRate(x)));
        RunOatSensitivityAnalysis(
            "savingsInterestRate",
            Ranges.SavingsInterestRateRange,
            x => Simulation.Run(savingsInterestRate: new InterestRate(x)));
        RunOatSensitivityAnalysis(
            "initialMonthlyRentPrice",
            Ranges.InitialMonthlyRentPrice,
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
            "mortgageInterestRate", Ranges.MortgageInterestRateRange,
            "savingsInterestRate", Ranges.SavingsInterestRateRange,
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
            "mortgageInterestRate", Ranges.MortgageInterestRateRange,
            "propertyValueYearlyIncrease", Ranges.PropertyValueYearlyIncrease,
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