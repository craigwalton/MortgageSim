using PropertySim.Variables;

namespace PropertySim.Experiments;

public static class SensitivityAnalysis
{
    public static void Run1D()
    {
        Run1D(
            "propertyValueYearlyIncrease",
            Ranges.PropertyValueYearlyIncrease,
            x => Simulation.Run(propertyValue: Baseline.PropertyValue with { YearlyIncrease = x }));
        Run1D(
            "mortgageInterestRate",
            Ranges.MortgageInterestRateRange,
            x => Simulation.Run(mortgageInterestRate: new InterestRate(x)));
        Run1D(
            "savingsInterestRate",
            Ranges.SavingsInterestRateRange,
            x => Simulation.Run(savingsInterestRate: new InterestRate(x)));
        Run1D(
            "initialMonthlyRentPrice",
            Ranges.InitialMonthlyRentPrice,
            x => Simulation.Run(rent: Baseline.RentPrice with {InitialMonthly = x}));
    }

    public static void Run2D()
    {
        Run2D(
            "mortgageInterestRate", Ranges.MortgageInterestRateRange,
            "savingsInterestRate", Ranges.SavingsInterestRateRange,
            (x, y) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                savingsInterestRate: new InterestRate(y)));
        Run2D(
            "initialPropertyValue", new Range(100_000m, 500_000m, 10_000m),
            "initialMonthlyRentPrice", new Range(500m, 3000m, 100m),
            (x, y) => Simulation.Run(
                propertyValue: Baseline.PropertyValue with { InitialValue = x },
                rent: Baseline.RentPrice with { InitialMonthly = y }));
        Run2D(
            "mortgageInterestRate", Ranges.MortgageInterestRateRange,
            "propertyValueYearlyIncrease", Ranges.PropertyValueYearlyIncrease,
            (x, y) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                propertyValue: Baseline.PropertyValue with {YearlyIncrease = y}));
        Run2D(
            "initialMonthlyRentPrice", Ranges.InitialMonthlyRentPrice,
            "rentPriceYearlyIncrease", Ranges.RentPriceYearlyIncrease,
            (x, y) => Simulation.Run(
                rent: new RentPrice(x, y)));
    }

    private static void Run1D(string variable, Range range, Func<decimal, Simulation.Result> run)
    {
        using var writer = new CsvWriter($"{variable}.csv", variable, "delta");
        for (var i = range.Start; i <= range.Stop; i += range.Step)
        {
            var result = run(i).ComputeDelta();
            writer.WriteLine(i, result);
        }
    }

    private static void Run2D(
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
}
