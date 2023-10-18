var yearlyMortgageInterestRate = new decimal(0.0209);
var monthlyMortgageInterestRate = yearlyMortgageInterestRate / 12;

var initialPropertyValue = new decimal(200_000);
var deposit = new decimal(50_000);
const int mortgageTermYears = 25;
var initialLoanValue = initialPropertyValue - deposit;
var monthlyPayment = ComputeMonthlyPayment();

var balance = initialPropertyValue - deposit;

for (var y = 0; y < mortgageTermYears; y++)
{
    for (var m = 0; m < 12; m++)
    {
        var interest = balance * monthlyMortgageInterestRate;
        var principal = monthlyPayment - interest;
        balance -= principal;
        Console.WriteLine($"M{m:00}/Y{y:00} payment: {monthlyPayment} (interest={interest}; principal={principal})");
    }
}

Console.WriteLine($"Final mortgage balance: {balance}");

decimal ComputeMonthlyPayment()
{
    var numberOfPayments = mortgageTermYears * 12;
    var interestRate = (double)monthlyMortgageInterestRate;
    var rateToPowerOfPayments = Math.Pow(1 + interestRate, numberOfPayments);
    var result = (double)initialLoanValue * (interestRate * rateToPowerOfPayments) / (rateToPowerOfPayments - 1);
    return new decimal(Math.Round(result, 2));
}