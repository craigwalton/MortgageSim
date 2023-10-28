namespace PropertySim.Variables;

public sealed class RentPrice
{
    public RentPrice(decimal initialMonthly, decimal yearlyIncrease)
    {
        MonthlyPrice = initialMonthly;
        YearlyIncrease = yearlyIncrease;
    }

    public decimal MonthlyPrice { get; private set; }

    public decimal YearlyIncrease { get; }

    public void ProcessYearlyUpdate()
    {
        MonthlyPrice *= 1 + YearlyIncrease;
    }
}
