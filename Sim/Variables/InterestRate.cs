namespace PropertySim.Variables;

public sealed class InterestRate
{
    private readonly decimal _yearlyInterest;

    public InterestRate(decimal yearlyInterest)
    {
        _yearlyInterest = yearlyInterest;
        ProcessYearlyUpdate();
    }

    public decimal Monthly { get; private set; }

    public void ProcessYearlyUpdate()
    {
        Monthly = _yearlyInterest / 12;
    }
}