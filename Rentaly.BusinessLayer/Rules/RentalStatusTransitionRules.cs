using Rentaly.EntityLayer.Enums;

namespace Rentaly.BusinessLayer.Rules;

public static class RentalStatusTransitionRules
{
    public static bool CanTransition(RentalStatus current, RentalStatus target) =>
        current switch
        {
            RentalStatus.Pending => target is RentalStatus.Approved or RentalStatus.Rejected or RentalStatus.Cancelled,
            RentalStatus.Approved => target is RentalStatus.Completed or RentalStatus.Cancelled,
            _ => false
        };
}
