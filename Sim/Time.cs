namespace PropertySim;

public sealed class Time
{
    private int _totalMonths;

    public void AdvanceOneMonth()
    {
        _totalMonths++;
    }

    public int Month => _totalMonths % 12 + 1;

    public int Year => _totalMonths / 12 + 1;

    public override string ToString()
    {
        return $"M{Month:00}/Y{Year:00}";
    }
}
