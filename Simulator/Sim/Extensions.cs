namespace PropertySim;

public static class Extensions
{
    public static decimal RaiseToPowerOf(this decimal value, int power)
    {
        switch (power)
        {
            case < 0:
                throw new ArgumentOutOfRangeException(nameof(power));
            case 0:
                return 1;
        }
        var result = value;
        while (power > 1)
        {
            result *= value;
            power--;
        }
        return result;
    }
}
