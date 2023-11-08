namespace MortgageSim.Sim.Variables;

/// <threadsafety static="true" instance="true"/>
public readonly record struct InterestRate(decimal Yearly)
{
    public decimal Monthly => Yearly / 12;
}