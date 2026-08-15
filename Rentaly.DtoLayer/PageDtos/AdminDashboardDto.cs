namespace Rentaly.DtoLayer.PageDtos;

public class AdminDashboardDto
{
    public int ActiveCarCount { get; set; }
    public int AvailableCarCount { get; set; }
    public int TotalRentalCount { get; set; }
    public int PendingRentalCount { get; set; }
    public int CompletedRentalCount { get; set; }
    public int CustomerCount { get; set; }
    public int TestimonialCount { get; set; }
    public int BranchCount { get; set; }
    public int NotificationCount => PendingRentalCount;
}
