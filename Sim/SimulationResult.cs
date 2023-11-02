namespace PropertySim;

public sealed record SimulationResult(decimal PurchaseEquity, decimal RentEquity)
{
    public decimal ComputeDelta()
    {
        return PurchaseEquity - RentEquity;
    }
}
