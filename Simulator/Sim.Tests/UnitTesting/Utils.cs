namespace MortgageSim.Sim.Tests.UnitTesting;

public static class Utils
{
    public static void Repeat(Action action, int times)
    {
        for (var i = 0; i < times; i++)
        {
            action();
        }
    }
}
