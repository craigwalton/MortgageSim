using System.Diagnostics;
using CommunityToolkit.Diagnostics;
using PropertySim.Plans;
using PropertySim.Variables;

namespace PropertySim;

/// <threadsafety static="true" instance="false"/>
public sealed class Simulation
{
    private readonly int _simulationDurationYears;
    private readonly decimal _deposit;
    private readonly int _mortgageTermYears;
    private readonly PropertyValue _propertyValue;
    private readonly InterestRate _mortgageInterestRate;
    private readonly RentPrice _rent;
    private readonly InterestRate _savingsInterestRate;

    public Simulation(
        int simulationDurationYears = Baseline.SimulationDurationYears,
        decimal deposit = Baseline.Deposit,
        PropertyValue? propertyValue = null,
        int mortgageTermYears = Baseline.MortgageTermYears,
        InterestRate? mortgageInterestRate = null,
        RentPrice? rent = null,
        InterestRate? savingsInterestRate = null)
    {
        _simulationDurationYears = simulationDurationYears;
        _deposit = deposit;
        _mortgageTermYears = mortgageTermYears;
        _propertyValue = propertyValue ?? Baseline.PropertyValue;
        _mortgageInterestRate = mortgageInterestRate ?? Baseline.MortgageInterestRate;
        _rent = rent ?? Baseline.RentPrice;
        _savingsInterestRate = savingsInterestRate ?? Baseline.SavingsInterestRate;
        Validate();
    }

    public SimulationResult Run()
    {
        var purchasePlan = new PurchasePlan(_propertyValue, _deposit, _mortgageTermYears, _mortgageInterestRate);
        var rentalPlan = new RentalPlan(_deposit, _rent, _savingsInterestRate);
        var time = RunCore(_simulationDurationYears, purchasePlan, rentalPlan);
        var result = new SimulationResult(purchasePlan.ComputeEquity(time), rentalPlan.ComputeEquity());
        Debug.WriteLine($"Purchase plan equity={result.PurchaseEquity:C}");
        Debug.WriteLine($"Rental plan equity={result.RentEquity:C}");
        Debug.WriteLine($"Delta={result.ComputeDelta():C}");
        return result;
    }

    private void Validate()
    {
        Guard.IsGreaterThan(_simulationDurationYears, 0);
        Guard.IsGreaterThanOrEqualTo(_deposit, 0m);
        Guard.IsGreaterThanOrEqualTo(_propertyValue.InitialValue, 0m);
        Guard.IsGreaterThan(_mortgageTermYears, 0);
        Guard.IsGreaterThanOrEqualTo(_rent.InitialMonthly, 0m);
    }

    private static Time RunCore(int simulationYears, PurchasePlan purchasePlan, RentalPlan rentalPlan)
    {
        Time time;
        for (time = new Time(); time.Year < simulationYears; time.AdvanceOneMonth())
        {
            Debug.WriteLine($"## {time} ##");
            purchasePlan.ProcessMonth(out var payment);
            rentalPlan.ProcessMonth(payment, time);
        }
        return time;
    }
}