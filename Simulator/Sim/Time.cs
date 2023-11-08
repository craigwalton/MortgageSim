using CommunityToolkit.Diagnostics;

namespace MortgageSim.Sim;

internal sealed class Time
{
    private int _totalMonths;

    public Time()
    {
    }

    public Time(int month, int year)
    {
        Guard.IsBetweenOrEqualTo(month, 0, 12);
        Guard.IsGreaterThanOrEqualTo(year, 0);

        _totalMonths = month + year * 12;
    }

    public void AdvanceOneMonth()
    {
        _totalMonths++;
    }

    public int Month => _totalMonths % 12;

    public int Year => _totalMonths / 12;

    public override string ToString()
    {
        return $"M{Month:00}/Y{Year:00}";
    }
}
