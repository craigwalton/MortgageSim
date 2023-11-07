namespace PropertySim;

/// <threadsafety static="true" instance="true"/>
public sealed record SimulationResult(decimal PurchaseEquity, decimal RentEquity)
{
    public decimal ComputeDelta()
    {
        return PurchaseEquity - RentEquity;
    }
}
