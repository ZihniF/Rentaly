using Rentaly.BusinessLayer.Rules;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.Tests;

public class RentalStatusTransitionRulesTests
{
    [Theory]
    [InlineData(RentalStatus.Pending, RentalStatus.Approved)]
    [InlineData(RentalStatus.Pending, RentalStatus.Rejected)]
    [InlineData(RentalStatus.Pending, RentalStatus.Cancelled)]
    [InlineData(RentalStatus.Approved, RentalStatus.Completed)]
    [InlineData(RentalStatus.Approved, RentalStatus.Cancelled)]
    public void CanTransition_AllowsSupportedLifecycleMoves(RentalStatus current, RentalStatus target)
    {
        Assert.True(RentalStatusTransitionRules.CanTransition(current, target));
    }

    [Theory]
    [InlineData(RentalStatus.Pending, RentalStatus.Completed)]
    [InlineData(RentalStatus.Approved, RentalStatus.Rejected)]
    [InlineData(RentalStatus.Rejected, RentalStatus.Approved)]
    [InlineData(RentalStatus.Cancelled, RentalStatus.Approved)]
    [InlineData(RentalStatus.Completed, RentalStatus.Cancelled)]
    public void CanTransition_RejectsUnsupportedAndTerminalMoves(RentalStatus current, RentalStatus target)
    {
        Assert.False(RentalStatusTransitionRules.CanTransition(current, target));
    }
}
