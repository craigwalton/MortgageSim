namespace PropertySim.Variables;

/// <threadsafety static="true" instance="true"/>
public sealed record PropertyValue(decimal InitialValue, decimal YearlyIncrease)
{
    public decimal ComputeValue(Time time)
    {
        return InitialValue * (YearlyIncrease + 1).RaiseToPowerOf(time.Year);
    }

}
