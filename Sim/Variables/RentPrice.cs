namespace PropertySim.Variables;

public sealed class RentPrice
{
    private readonly decimal _yearlyIncrease;

    public RentPrice(decimal initialMonthly, decimal yearlyIncrease)
    {
        MonthlyPrice = initialMonthly;
        _yearlyIncrease = yearlyIncrease;
    }

    public decimal MonthlyPrice { get; private set; }

    public void ProcessYearlyUpdate()
    {
        MonthlyPrice *= 1 + _yearlyIncrease;
    }
}
