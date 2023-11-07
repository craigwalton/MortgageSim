using PropertySim;
using PropertySim.Variables;
using Xunit;

namespace Sim.Tests;

public sealed class SimulationTests
{
    [Fact]
    public void Verify_ground_truth_1()
    {
        var sut = new Simulation(
            simulationYears: 5,
            deposit: 30_000m,
            propertyValue: new PropertyValue(200_000, 0.03m),
            mortgageInterestRate: new InterestRate(0.035m),
            rent: new RentPrice(700m, 0.03m),
            savingsInterestRate: new InterestRate(0.05m));

        var actual = sut.Run();

        Assert.Equal(85_110.13m, actual.PurchaseEquity, precision: 2);
        Assert.Equal(45_979.34m, actual.RentEquity, precision: 2);
        Assert.Equal(39_130.79m, actual.ComputeDelta(), precision: 2);
    }

    [Fact]
    public void Handles_zero_interest()
    {
        var sut = new Simulation(
            simulationYears: 1,
            mortgageInterestRate: new InterestRate(0m),
            savingsInterestRate: new InterestRate(0m));

        sut.Run();
    }
}
