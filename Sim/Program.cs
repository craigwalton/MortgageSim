﻿using PropertySim.Experiments;
using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunBaseline();
        // SensitivityAnalysis.Run1D();
        // SensitivityAnalysis.Run2D();
        // SensitivityAnalysis.Run3D();
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