namespace PropertySim.Plans;

public abstract class Plan
{
    public abstract void ProcessMonth(decimal income);

    public abstract decimal ComputeEquity();
}