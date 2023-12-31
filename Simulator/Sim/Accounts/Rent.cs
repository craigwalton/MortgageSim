using System.Diagnostics;
using MortgageSim.Sim.Variables;

namespace MortgageSim.Sim.Accounts;

internal sealed class Rent
{
    private readonly RentPrice _rentPrice;

    public Rent(RentPrice rentPrice)
    {
        _rentPrice = rentPrice;
    }

    public decimal TakeMonthlyPayment(Time time)
    {
        var price = _rentPrice.ComputeMonthlyPrice(time);
        Debug.WriteLine($"Rent payment={price:C}");
        return price;
    }
}
