﻿using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        Run1DSensitivityAnalysis();
    }

    private static void Run1DSensitivityAnalysis()
    {
        using var sw = new StreamWriter("../../../../data/1d.csv");
        sw.WriteLine("mortgageInterestRate,delta");
        for (var i = 0m; i <= 0.2m; i += 0.01m)
        {
            var mortgageInterestRate = new InterestRate(i);
            var result = new Simulation().Run(
                mortgageInterestRate: mortgageInterestRate,
                simulationYears: 5).ComputeDelta();
            sw.WriteLine($"{i},{result}");
        }
    }

    private static void Run2DSensitivityAnalysis()
    {
        using var sw = new StreamWriter("../../../../data/2d.csv");
        sw.WriteLine("interestRate,initialRent,delta");
        for (var i = -0.05m; i <= 0.2m; i += 0.01m)
        {
            for (var r = 500m; r <= 2000m; r += 100m)
            {
                var mortgageInterestRate = new InterestRate(i + 0.02m);
                var savingsInterestRate = new InterestRate(i);
                var result = new Simulation().Run(
                    mortgageInterestRate: mortgageInterestRate,
                    savingsInterestRate: savingsInterestRate,
                    rent: new RentPrice(r, Baseline.RentPrice.YearlyIncrease),
                    simulationYears: 5);
                sw.WriteLine($"{i},{r},{result.ComputeDelta():F2}");
            }
        }
    }

    private static void Run3DSensitivityAnalysis()
    {
        using var sw = new StreamWriter("../../../../data/3d.csv");
        sw.WriteLine("interestRate,propertyValueIncrease,rentIncrease,delta");
        for (var i = -0.05m; i <= 0.2m; i += 0.01m)
        {
            for (var p = -0.1m; p <= 0.1m; p += 0.01m)
            {
                for (var r = 0.0m; r <= 0.1m; r += 0.01m)
                {
                    var mortgageInterestRate = new InterestRate(i + 0.02m);
                    var savingsInterestRate = new InterestRate(i);
                    var result = new Simulation().Run(
                        mortgageInterestRate: mortgageInterestRate,
                        savingsInterestRate: savingsInterestRate,
                        rent: new RentPrice(Baseline.RentPrice.MonthlyPrice, r),
                        propertyValue: new PropertyValue(Baseline.PropertyValue.Value, p),
                        simulationYears: 5);
                    sw.WriteLine($"{i},{p},{r},{result.ComputeDelta():F2}");
                }
            }
        }
    }

    private static void RunSim()
    {
        const int count = 1;
        var results = new List<Simulation.Result>(count);
        for (var i = 0; i < count; i++)
        {
            var simulation = new Simulation();
            var result = simulation.Run();
            results.Add(result);
        }
        var purchaseAverageEquity = results.Average(x => x.PurchaseEquity);
        var rentAverageEquity = results.Average(x => x.RentEquity);
        Console.WriteLine($"Avg Purchase equity={purchaseAverageEquity:C}; Avg Rent equity={rentAverageEquity:C}");
    }

    private static StreamWriter CreateConsoleStreamWriter()
    {
        var output = new StreamWriter(Console.OpenStandardOutput());
        output.AutoFlush = true;
        return output;
    }
}