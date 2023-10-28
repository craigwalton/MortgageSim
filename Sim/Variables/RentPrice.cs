namespace PropertySim.Variables;

/// <threadsafety static="true" instance="true"/>
public readonly record struct RentPrice(decimal InitialMonthly, decimal YearlyIncrease)
{
    public decimal ComputeMonthlyPrice(Time time)
    {
        return InitialMonthly * (YearlyIncrease + 1).RaiseToPowerOf(time.Year);
    }
}
