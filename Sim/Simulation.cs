using System.Diagnostics;
using PropertySim.Plans;
using PropertySim.Variables;

namespace PropertySim;

public static class Simulation
{
    public static Result Run(
        int simulationYears = Baseline.SimulationYears,
        decimal deposit = Baseline.Deposit,
        int mortgageTermYears = Baseline.MortgageTermYears,
        PropertyValue? propertyValue = null,
        InterestRate? mortgageInterestRate = null,
        RentPrice? rent = null,
        InterestRate? savingsInterestRate = null)
    {
        propertyValue ??= Baseline.PropertyValue;
        mortgageInterestRate ??= Baseline.MortgageInterestRate;
        rent ??= Baseline.RentPrice;
        savingsInterestRate ??= Baseline.SavingsInterestRate;

        var purchasePlan = new HousePurchasePlan(
            propertyValue.Value,
            deposit,
            mortgageTermYears,
            mortgageInterestRate.Value);
        var rentalPlan = new HouseRentalPlan(deposit, rent.Value, savingsInterestRate.Value);

        Time time;
        for (time = new Time(); time.Year < simulationYears; time.AdvanceOneMonth())
        {
            Debug.WriteLine($"## {time} ##");
            purchasePlan.ProcessMonth(out var payment);
            rentalPlan.ProcessMonth(payment, time);
        }

        Debug.WriteLine($"Purchase plan: Equity={purchasePlan.ComputeEquity(time):C}");
        Debug.WriteLine($"Rental plan: Equity={rentalPlan.ComputeEquity(time):C}");

        return new Result(purchasePlan.ComputeEquity(time), rentalPlan.ComputeEquity(time));
    }

    public readonly record struct Result(decimal PurchaseEquity, decimal RentEquity)
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
