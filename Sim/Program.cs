using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunBaseline();
        RunMortgageInterestRateSensitivityAnalysis();
        RunSavingsInterestRateSensitivityAnalysis();
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

    private static void RunMortgageInterestRateSensitivityAnalysis()
    {
        using var writer = new CsvWriter("mortgageInterestRate.csv", "mortgageInterestRate", "delta");
        for (var i = 0m; i <= 0.2m; i += 0.01m)
        {
            var result = Simulation.Run(mortgageInterestRate: new InterestRate(i))
                .ComputeDelta();
            writer.WriteLine(i, result);
        }
    }

    private static void RunSavingsInterestRateSensitivityAnalysis()
    {
        using var writer = new CsvWriter("savingsInterestRate.csv", "savingsInterestRate", "delta");
        for (var i = -0.1m; i <= 0.2m; i += 0.01m)
        {
            var result = Simulation.Run(savingsInterestRate: new InterestRate(i))
                .ComputeDelta();
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