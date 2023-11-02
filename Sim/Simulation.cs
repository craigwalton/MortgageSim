using System.Diagnostics;
using PropertySim.Plans;
using PropertySim.Variables;

namespace PropertySim;

public sealed class Simulation
{
    private readonly int _simulationYears;
    private readonly decimal _deposit;
    private readonly int _mortgageTermYears;
    private readonly PropertyValue _propertyValue;
    private readonly InterestRate _mortgageInterestRate;
    private readonly RentPrice _rent;
    private readonly InterestRate _savingsInterestRate;

    public Simulation(
        int simulationYears = Baseline.SimulationYears,
        decimal deposit = Baseline.Deposit,
        int mortgageTermYears = Baseline.MortgageTermYears,
        PropertyValue? propertyValue = null,
        InterestRate? mortgageInterestRate = null,
        RentPrice? rent = null,
        InterestRate? savingsInterestRate = null)
    {
        _simulationYears = simulationYears;
        _deposit = deposit;
        _mortgageTermYears = mortgageTermYears;
        _propertyValue = propertyValue ?? Baseline.PropertyValue;
        _mortgageInterestRate = mortgageInterestRate ?? Baseline.MortgageInterestRate;
        _rent = rent ?? Baseline.RentPrice;
        _savingsInterestRate = savingsInterestRate ?? Baseline.SavingsInterestRate;
    }

    public Result Run()
    {
        var purchasePlan = new HousePurchasePlan(_propertyValue, _deposit, _mortgageTermYears, _mortgageInterestRate);
        var rentalPlan = new HouseRentalPlan(_deposit, _rent, _savingsInterestRate);
        var time = RunCore(_simulationYears, purchasePlan, rentalPlan);
        Debug.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity(time):C}");
        Debug.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity(time):C}");
        return new Result(purchasePlan.ComputeEquity(time), rentalPlan.ComputeEquity(time));
    }

    private static Time RunCore(int simulationYears, HousePurchasePlan purchasePlan, HouseRentalPlan rentalPlan)
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

    public sealed record Result(decimal PurchaseEquity, decimal RentEquity)
    {
        public override string ToString()
        {
            return $"Purchase equity={PurchaseEquity:C}; Rent equity={RentEquity:C}";
        }

        public decimal ComputeDelta()
        {
            return PurchaseEquity - RentEquity;
        }
    }
}
