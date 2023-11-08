namespace MortgageSim.Sim.Variables;

/// <threadsafety static="true" instance="true"/>
public sealed record RentPrice(decimal InitialMonthly, decimal YearlyIncrease)
{
    internal decimal ComputeMonthlyPrice(Time time)
    {
        return InitialMonthly * (YearlyIncrease + 1).RaiseToPowerOf(time.Year);
    }
}
