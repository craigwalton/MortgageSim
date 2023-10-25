using MathNet.Numerics.Distributions;

namespace PropertySim;

public static class Distributions
{
    public static Normal Constant(double value)
    {
        return new Normal(value, 0.0);
    }

    public static Normal Constant(decimal value)
    {
        return new Normal((double)value, 0.0);
    }
}
