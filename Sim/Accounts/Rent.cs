using System.Diagnostics;
using PropertySim.Variables;

namespace PropertySim.Accounts;

public sealed class Rent
{
    private readonly RentPrice _rentPrice;

    public Rent(RentPrice rentPrice)
    {
        _rentPrice = rentPrice;
    }

    public decimal TakePayment(Time time)
    {
        var price = _rentPrice.ComputeMonthlyPrice(time);
        Debug.WriteLine($"Rent payment={price:C}");
        return price;
    }
}
