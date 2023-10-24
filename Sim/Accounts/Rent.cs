namespace PropertySim.Accounts;

public class Rent
{
    private readonly StreamWriter _output;

    public Rent(StreamWriter output)
    {
        _output = output;
    }

    public void MakePayment(decimal payment)
    {
        _output.WriteLine($"Rent payment={payment:C}");
    }
}
