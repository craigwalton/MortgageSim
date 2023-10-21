﻿using MathNet.Numerics.Distributions;
using PropertySim.Plans;

namespace PropertySim;

internal static class Sim
{
    public static void Main()
    {
        const decimal initialPropertyValue = 200_000m;
        const decimal deposit = 50_000m;
        const int mortgageTermYears = 25;
        const decimal income = 1000m;
        // TODO: consider modelling these rates as offsets from the base rate.
        var mortgageInterestRate = new InterestRate(new Normal(0.0209, 0.01));
        var savingsInterestRate = new InterestRate(new Normal(0.001, 0.01));
        var rent = new RentPrice(500m, new Normal(1.02, 0.02));

        var output = new StreamWriter(Console.OpenStandardOutput());
        output.AutoFlush = true;
        // output = StreamWriter.Null;

        var purchasePlan = new HousePurchasePlan(
            initialPropertyValue,
            deposit,
            mortgageTermYears,
            mortgageInterestRate,
            savingsInterestRate,
            output);
        var rentalPlan = new HouseRentalPlan(deposit, rent, savingsInterestRate, output);

        for (var y = 0; y < 10; y++)
        {
            for (var m = 0; m < 12; m++)
            {
                output.WriteLine($"M{m:00}/Y{y:00}");
                purchasePlan.ProcessMonth(income);
                rentalPlan.ProcessMonth(income);
            }
            mortgageInterestRate.ProcessYearlyUpdate();
            savingsInterestRate.ProcessYearlyUpdate();
            rent.ProcessYearlyUpdate();
        }

        output.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity():C}");
        output.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity():C}");
    }
}