using PropertySim;
using PropertySim.Variables;
using Xunit;

namespace Sim.Tests;

public class SimTests
{
    [Fact]
    public void Verify_ground_truth_1()
    {
        var sut = new Simulation();

        var actual = sut.Run(
            deposit: 30_000m,
            mortgageInterestRate: new InterestRate(0.035m),
            savingsInterestRate: new InterestRate(0.05m),
            rent: new RentPrice(700m, 0.03m),
            propertyValue: new PropertyValue(200_000, 0.03m),
            simulationYears: 5);

        Assert.Equal(85_110.12m, actual.PurchaseEquity, precision: 2);
        Assert.Equal(45_979.33m, actual.RentEquity, precision: 2);
        Assert.Equal(39_130.79m, actual.ComputeDelta(), precision: 2);
    }

    [Fact]
    public void Handles_zero_interest()
    {
        var sut = new Simulation();

        sut.Run(
            mortgageInterestRate: new InterestRate(0m),
            savingsInterestRate: new InterestRate(0m),
            simulationYears: 1);
    }
}
