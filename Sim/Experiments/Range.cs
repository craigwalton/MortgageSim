namespace PropertySim.Experiments;

public readonly record struct Range(decimal Start, decimal Stop, decimal Step)
{
    public IEnumerable<decimal> Enumerate()
    {
        for (var i = Start; i <= Stop; i += Step)
        {
            yield return i;
        }
    }
}
