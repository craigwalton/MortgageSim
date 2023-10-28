namespace PropertySim.Variables;

public class PropertyValue
{
    private readonly decimal _yearlyIncrease;

    public PropertyValue(decimal initialValue, decimal yearlyIncrease)
    {
        Value = initialValue;
        _yearlyIncrease = yearlyIncrease;
    }

    public decimal Value { get; private set; }

    public void ProcessYearlyUpdate()
    {
        Value *= 1 + _yearlyIncrease;
    }
}
