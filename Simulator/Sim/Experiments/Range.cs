namespace PropertySim.Experiments;

public sealed record Range(decimal Start, decimal Stop, decimal Step)
{
    public IEnumerable<decimal> Enumerate()
    {
        for (var i = Start; i <= Stop; i += Step)
        {
            yield return i;
        }
    }
}
