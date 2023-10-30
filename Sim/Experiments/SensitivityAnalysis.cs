using PropertySim.Variables;

namespace PropertySim.Experiments;

public static class SensitivityAnalysis
{
    private static readonly Variable InitialPropertyValue = new("initialPropertyValue", new Range(100_000m, 500_000m, 10_000m));
    private static readonly Variable PropertyValueYearlyIncrease = new("propertyValueYearlyIncrease", new(-0.15m, 0.15m, 0.01m));
    private static readonly Variable MortgageInterestRate = new("mortgageInterestRate", new(-0m, 0.2m, 0.01m));
    private static readonly Variable InitialMonthlyRentPrice = new("initialMonthlyRentPrice", new(500m, 2000m, 100m));
    private static readonly Variable RentPriceYearlyIncrease = new("rentPriceYearlyIncrease", new(-0.1m, 0.15m, 0.01m));
    private static readonly Variable SavingsInterestRate = new("savingsInterestRate", new(-0.1m, 0.2m, 0.01m));

    public static void Run1D()
    {
        Run1D(
            PropertyValueYearlyIncrease,
            x => Simulation.Run(propertyValue: Baseline.PropertyValue with { YearlyIncrease = x }));
        Run1D(
            MortgageInterestRate,
            x => Simulation.Run(mortgageInterestRate: new InterestRate(x)));
        Run1D(
            InitialMonthlyRentPrice,
            x => Simulation.Run(rent: Baseline.RentPrice with {InitialMonthly = x}));
        Run1D(
            SavingsInterestRate,
            x => Simulation.Run(savingsInterestRate: new InterestRate(x)));
    }

    public static void Run2D()
    {
        Run2D(
            MortgageInterestRate,
            SavingsInterestRate,
            (x, y) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                savingsInterestRate: new InterestRate(y)));
        Run2D(
            InitialPropertyValue,
            InitialMonthlyRentPrice with {Range = new Range(500m, 3000m, 100m)},
            (x, y) => Simulation.Run(
                propertyValue: Baseline.PropertyValue with { InitialValue = x },
                rent: Baseline.RentPrice with { InitialMonthly = y }));
        Run2D(
            MortgageInterestRate,
            PropertyValueYearlyIncrease,
            (x, y) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                propertyValue: Baseline.PropertyValue with {YearlyIncrease = y}));
        Run2D(
            InitialMonthlyRentPrice,
            RentPriceYearlyIncrease,
            (x, y) => Simulation.Run(
                rent: new RentPrice(x, y)));
    }

    public static void Run3D()
    {
        Run3D(
            MortgageInterestRate,
            PropertyValueYearlyIncrease,
            InitialMonthlyRentPrice,
            (x, y, z) => Simulation.Run(
                mortgageInterestRate: new InterestRate(x),
                propertyValue: Baseline.PropertyValue with {YearlyIncrease = y},
                rent: Baseline.RentPrice with { InitialMonthly = z }));
    }

    private static void Run1D(Variable x, Func<decimal, Simulation.Result> run)
    {
        using var writer = new CsvWriter($"{x.Name}.csv", x.Name, "delta");
        foreach (var i in x.Range.Enumerate())
        {
            var result = run(i).ComputeDelta();
            writer.WriteLine(i, result);
        }
    }

    private static void Run2D(Variable x, Variable y, Func<decimal, decimal, Simulation.Result> run)
    {
        using var writer = new CsvWriter($"{x.Name}-{y.Name}.csv", x.Name, y.Name, "delta");
        foreach (var i in x.Range.Enumerate())
        {
            foreach (var j in y.Range.Enumerate())
            {
                var result = run(i, j).ComputeDelta();
                writer.WriteLine(i, j, result);
            }
        }
    }

    private static void Run3D(
        Variable x,
        Variable y,
        Variable z,
        Func<decimal, decimal, decimal, Simulation.Result> run)
    {
        using var writer = new CsvWriter($"{x.Name}-{y.Name}-{z.Name}.csv", x.Name, y.Name, z.Name, "delta");
        foreach (var i in x.Range.Enumerate())
        {
            foreach (var j in y.Range.Enumerate())
            {
                foreach (var k in z.Range.Enumerate())
                {
                    var result = run(i, j, k).ComputeDelta();
                    writer.WriteLine(i, j, k, result);
                }
            }
        }
    }

    private sealed record Variable(string Name, Range Range);
}
