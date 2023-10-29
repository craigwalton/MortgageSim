using PropertySim.Experiments;
using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunBaseline();
        SensitivityAnalysis.Run1D();
        SensitivityAnalysis.Run2D();
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