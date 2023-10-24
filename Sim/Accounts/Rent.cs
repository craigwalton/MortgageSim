using PropertySim.Variables;

namespace PropertySim.Accounts;

public class Rent
{
    private readonly RentPrice _rentPrice;
    private readonly StreamWriter _output;

    public Rent(RentPrice rentPrice, StreamWriter output)
    {
        _rentPrice = rentPrice;
        _output = output;
    }

    public decimal TakePayment()
    {
        _output.WriteLine($"Rent payment={_rentPrice.MonthlyPrice:C}");
        return _rentPrice.MonthlyPrice;
    }
}
