using PropertySim.Csv;
using PropertySim.Experiments;

namespace PropertySim;

internal static class Program
{
    public static void Main()
    {
        RunBaseline();
        SensitivityAnalysis.Run1D();
        SensitivityAnalysis.Run2D();
        SensitivityAnalysis.Run3D();
    }

    private static void RunBaseline()
    {
        using var writer = new Writer(
            "baseline.csv",
            Columns.SimulationYears,
            Columns.Deposit,
            Columns.InitialPropertyValue,
            Columns.PropertyValueYearlyIncrease,
            Columns.MortgageTermYears,
            Columns.MortgageInterestRate,
            Columns.InitialMonthlyRentPrice,
            Columns.RentPriceYearlyIncrease,
            Columns.SavingsInterestRate,
            Columns.Delta);
        var result = new Simulation().Run();
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
}