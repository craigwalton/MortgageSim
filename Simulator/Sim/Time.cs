namespace PropertySim;

public sealed class Time
{
    private int _totalMonths;

    public Time()
    {
    }

    public Time(int month, int year)
    {
        if (month is < 0 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }
        if (year < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(year));
        }
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
