using System.CommandLine;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;
using PropertySim;
using PropertySim.Variables;

namespace Sim.Cli;

public static class Program
{
    public static int Main(string[] args)
    {
        var currency = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
        var simulationDuration = new Option<int>(
            name: "--simulationDuration",
            description: "The simulation's duration, specified in years.",
            getDefaultValue: () => Baseline.SimulationDurationYears);
        var deposit = new Option<decimal>(
            name: "--deposit",
            description: $"The deposit, specified in {currency}.",
            getDefaultValue: () => Baseline.Deposit);
        var initialPropertyValue = new Option<decimal>(
            name: "--initialPropertyValue",
            description: $"The property's initial value, specified in {currency}.",
            getDefaultValue: () => Baseline.PropertyValue.InitialValue);
        var propertyValueYearlyIncrease = new Option<decimal>(
            name: "--propertyValueYearlyIncrease",
            description: "The property's yearly increase in value (can be negative), e.g. 1% = 0.01.",
            getDefaultValue: () => Baseline.PropertyValue.YearlyIncrease);
        var mortgageTerm = new Option<int>(
            name: "--mortgageTerm",
            description: "The mortgage term, specified in years.",
            getDefaultValue: () => Baseline.MortgageTermYears);
        var mortgageInterestRate = new Option<decimal>(
            name: "--mortgageInterestRate",
            description: "The mortgage's yearly interest rate, e.g. 1% = 0.01.",
            getDefaultValue: () => Baseline.MortgageInterestRate.Yearly);
        var initialRentPrice = new Option<decimal>(
            name: "--initialRentPrice",
            description: $"The initial monthly rent, specified in {currency}.",
            getDefaultValue: () => Baseline.RentPrice.InitialMonthly);
        var rentPriceYearlyIncrease = new Option<decimal>(
            name: "--rentPriceYearlyIncrease",
            description: "The rent's yearly rate of increase (can be negative), e.g. 1% = 0.01.",
            getDefaultValue: () => Baseline.RentPrice.YearlyIncrease);
        var savingsInterestRate = new Option<decimal>(
            name: "--savingsInterestRate",
            description: "The savings yearly interest rate (can be negative), e.g. 1% = 0.01.",
            getDefaultValue: () => Baseline.SavingsInterestRate.Yearly);

        var rootCommand = new RootCommand("Mortgage Simulator CLI")
        {
            simulationDuration,
            deposit,
            initialPropertyValue,
            propertyValueYearlyIncrease,
            mortgageTerm,
            mortgageInterestRate,
            initialRentPrice,
            rentPriceYearlyIncrease,
            savingsInterestRate,
        };
        rootCommand.SetHandler(
            Run,
            simulationDuration,
            deposit,
            initialPropertyValue,
            propertyValueYearlyIncrease,
            mortgageTerm,
            mortgageInterestRate,
            initialRentPrice,
            rentPriceYearlyIncrease,
            savingsInterestRate);

        return rootCommand.InvokeAsync(args).Result;
    }

    private static void Run(
        int simulationDuration,
        decimal deposit,
        decimal initialPropertyValue,
        decimal propertyValueYearlyIncrease,
        int mortgageTerm,
        decimal mortgageInterestRate,
        decimal initialRentPrice,
        decimal rentPriceYearlyIncrease,
        decimal savingsInterestRate)
    {
        Console.WriteLine("Running simulation with parameters:");
        Print(simulationDuration, "i");
        var result = new Simulation(
                simulationDuration,
                deposit,
                new PropertyValue(initialPropertyValue, propertyValueYearlyIncrease),
                    mortgageTerm,
                new InterestRate(mortgageInterestRate),
                new RentPrice(initialRentPrice, rentPriceYearlyIncrease),
                new InterestRate(savingsInterestRate))
            .Run();
        Console.WriteLine($"{result.ComputeDelta():C}");
    }

    private static void Print<T>(T value, string format, [CallerArgumentExpression("value")] string? paramName = null)
    {
        Guard.IsNotNull(paramName);
        Console.WriteLine($"{paramName,-30}: {string.Format(format, value)}");
    }
}