namespace PropertySim.Plans;

public abstract class Plan
{
    public abstract void ProcessMonth(
        decimal income,
        InterestRate mortgageInterestRate,
        InterestRate savingsInterestRate);

    public abstract decimal ComputeEquity();
}