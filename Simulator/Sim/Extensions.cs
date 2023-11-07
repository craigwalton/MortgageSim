using CommunityToolkit.Diagnostics;

namespace PropertySim;

public static class Extensions
{
    public static decimal RaiseToPowerOf(this decimal value, int power)
    {
        Guard.IsGreaterThanOrEqualTo(power, 0);

        if (power == 0)
        {
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
