using MortgageSim.Sim.Csv;
using MortgageSim.Sim.Variables;

namespace MortgageSim.Sim.Experiments;

internal static class SensitivityAnalysis
{
    private static readonly Variable s_initialPropertyValue = new(Columns.InitialPropertyValue, new(100_000m, 500_000m, 10_000m));
    private static readonly Variable s_propertyValueYearlyIncrease = new(Columns.PropertyValueYearlyIncrease, new(-0.15m, 0.15m, 0.01m));
    private static readonly Variable s_mortgageInterestRate = new(Columns.MortgageInterestRate, new(-0m, 0.2m, 0.01m));
    private static readonly Variable s_initialMonthlyRentPrice = new(Columns.InitialMonthlyRentPrice, new(500m, 2000m, 100m));
    private static readonly Variable s_rentPriceYearlyIncrease = new(Columns.RentPriceYearlyIncrease, new(-0.1m, 0.15m, 0.01m));
    private static readonly Variable s_savingsInterestRate = new(Columns.SavingsInterestRate, new(-0.1m, 0.2m, 0.01m));

    public static void Run1D()
    {
        Run1D(
            s_initialPropertyValue,
            x => new Simulation(propertyValue: Baseline.PropertyValue with { InitialValue = x }));
        Run1D(
            s_propertyValueYearlyIncrease,
            x => new Simulation(propertyValue: Baseline.PropertyValue with { YearlyIncrease = x }));
        Run1D(
            s_mortgageInterestRate,
            x => new Simulation(mortgageInterestRate: new InterestRate(x)));
        Run1D(
            s_initialMonthlyRentPrice,
            x => new Simulation(rent: Baseline.RentPrice with {InitialMonthly = x}));
        Run1D(
            s_rentPriceYearlyIncrease,
            x => new Simulation(rent: Baseline.RentPrice with {YearlyIncrease = x}));
        Run1D(
            s_savingsInterestRate,
            x => new Simulation(savingsInterestRate: new InterestRate(x)));
    }

    public static void Run2D()
    {
        Run2D(
            s_mortgageInterestRate,
            s_savingsInterestRate,
            (x, y) => new Simulation(
                mortgageInterestRate: new InterestRate(x),
                savingsInterestRate: new InterestRate(y)));
        Run2D(
            s_initialPropertyValue,
            s_initialMonthlyRentPrice with {Range = new(500m, 3000m, 100m)},
            (x, y) => new Simulation(
                propertyValue: Baseline.PropertyValue with { InitialValue = x },
                rent: Baseline.RentPrice with { InitialMonthly = y }));
        Run2D(
            s_mortgageInterestRate,
            s_propertyValueYearlyIncrease,
            (x, y) => new Simulation(propertyValue: Baseline.PropertyValue with {YearlyIncrease = y}, mortgageInterestRate: new InterestRate(x)));
        Run2D(
            s_initialMonthlyRentPrice,
            s_rentPriceYearlyIncrease,
            (x, y) => new Simulation(
                rent: new RentPrice(x, y)));
    }

    public static void Run3D()
    {
        Run3D(
            s_mortgageInterestRate,
            s_propertyValueYearlyIncrease,
            s_initialMonthlyRentPrice,
            (x, y, z) => new Simulation(propertyValue: Baseline.PropertyValue with {YearlyIncrease = y},
                mortgageInterestRate: new InterestRate(x), rent: Baseline.RentPrice with { InitialMonthly = z }));
    }

    private static void Run1D(Variable x, Func<decimal, Simulation> create)
    {
        using var writer = Writer.Create($"{x.Name}.csv", x.Name, Columns.Delta);
        foreach (var i in x.Range.Enumerate())
        {
            var result = create(i).Run().ComputeDelta();
            writer.WriteLine(i, result);
        }
    }

    private static void Run2D(Variable x, Variable y, Func<decimal, decimal, Simulation> create)
    {
        using var writer = Writer.Create($"{x.Name}-{y.Name}.csv", x.Name, y.Name, Columns.Delta);
        foreach (var i in x.Range.Enumerate())
        {
            foreach (var j in y.Range.Enumerate())
            {
                var result = create(i, j).Run().ComputeDelta();
                writer.WriteLine(i, j, result);
            }
        }
    }

    private static void Run3D(Variable x, Variable y, Variable z, Func<decimal, decimal, decimal, Simulation> create)
    {
        using var writer = Writer.Create($"{x.Name}-{y.Name}-{z.Name}.csv", x.Name, y.Name, z.Name, Columns.Delta);
        foreach (var i in x.Range.Enumerate())
        {
            foreach (var j in y.Range.Enumerate())
            {
                foreach (var k in z.Range.Enumerate())
                {
                    var result = create(i, j, k).Run().ComputeDelta();
                    writer.WriteLine(i, j, k, result);
                }
            }
        }
    }

    private sealed record Variable(string Name, Range Range);
}
