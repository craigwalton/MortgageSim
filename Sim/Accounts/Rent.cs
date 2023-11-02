using PropertySim.Variables;

namespace PropertySim.Accounts;

public sealed class Rent
{
    private readonly RentPrice _rentPrice;
    private readonly StreamWriter _output;

    public Rent(RentPrice rentPrice, StreamWriter output)
    {
        _rentPrice = rentPrice;
        _output = output;
    }

    public decimal TakePayment(Time time)
    {
        var price = _rentPrice.ComputeMonthlyPrice(time);
        _output.WriteLine($"Rent payment={price:C}");
        return price;
    }
}
