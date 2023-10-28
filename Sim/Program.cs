﻿using PropertySim.Variables;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        RunSim();
    }

    private static void RunSensitivityAnalysis()
    {
        for (var i = 0m; i <= 0.2m; i += 0.01m)
        {
            var mortgageInterestRate = new InterestRate(i + 0.02m);
            var savingsInterestRate = new InterestRate(i);
            var result = new Simulation().Run(
                mortgageInterestRate: mortgageInterestRate,
                savingsInterestRate: savingsInterestRate,
                rent: new RentPrice(700m, 0.03m),
                propertyValue: new PropertyValue(200_000, 0.03m),
                simulationYears: 5);
            Console.WriteLine($"Interest: {i:F2}; {result.ComputeDelta():C}");
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
                    rent: new RentPrice(r, 0.03m),
                    propertyValue: new PropertyValue(200_000, 0.03m),
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
                        rent: new RentPrice(700m, r),
                        propertyValue: new PropertyValue(200_000, p),
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
            var result = simulation.Run(
                mortgageInterestRate: new InterestRate(0.0209m),
                savingsInterestRate: new InterestRate(0.001m),
                rent: new RentPrice(500m, 0.02m),
                propertyValue: new PropertyValue(200_000m, 0.03m));
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