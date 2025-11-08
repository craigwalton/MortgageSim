namespace MortgageSim.Sim.Variables;

/// <threadsafety static="true" instance="true"/>
/// <summary>
/// Represents a property's monetary value and its expected change over time.
/// </summary>
/// <param name="InitialValue">The property's initial monetary value e.g. 100,000.</param>
/// <param name="YearlyIncrease">The year-on-year growth as a proportion of the initial value e.g. 0.01 represents a
/// yearly growth of 1%. Growth can be negative or zero.</param>
public sealed record PropertyValue(decimal InitialValue, decimal YearlyIncrease)
{
    internal decimal ComputeValue(Time time)
    {
        return InitialValue * (YearlyIncrease + 1).RaiseToPowerOf(time.Year);
    }
}
