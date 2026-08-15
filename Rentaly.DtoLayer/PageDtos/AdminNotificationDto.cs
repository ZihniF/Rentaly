using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.DtoLayer.PageDtos;

public class AdminNotificationDto
{
    public int PendingCount { get; set; }
    public List<ResultRentalDto> LatestPendingRentals { get; set; } = [];
}
