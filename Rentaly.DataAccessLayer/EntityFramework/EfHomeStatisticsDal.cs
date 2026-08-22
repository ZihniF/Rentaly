using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DtoLayer.HomeDtos;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.DataAccessLayer.EntityFramework;

public class EfHomeStatisticsDal : IHomeStatisticsDal
{
    private readonly RentalyContext _context;

    public EfHomeStatisticsDal(RentalyContext context) => _context = context;

    public async Task<HomeStatisticsDto> GetAsync()
    {
        var rentalCounts = await _context.Rentals.AsNoTracking()
            .GroupBy(_ => 1)
            .Select(group => new
            {
                Key = group.Key,
                Total = group.Count(),
                Completed = group.Count(x => x.Status == RentalStatus.Completed),
                Pending = group.Count(x => x.Status == RentalStatus.Pending)
            })
            .OrderBy(x => x.Key)
            .FirstOrDefaultAsync();

        return new HomeStatisticsDto
        {
            CompletedRentalCount = rentalCounts?.Completed ?? 0,
            TotalRentalCount = rentalCounts?.Total ?? 0,
            PendingRentalCount = rentalCounts?.Pending ?? 0,
            CustomerCount = await _context.Customers.AsNoTracking().CountAsync(x => x.IsActive),
            ActiveCarCount = await _context.Cars.AsNoTracking().CountAsync(x => x.IsActive),
            AvailableCarCount = await _context.Cars.AsNoTracking()
                .CountAsync(x => x.IsActive && x.IsAvailable),
            TestimonialCount = await _context.HomeContents.AsNoTracking()
                .CountAsync(x => x.Section == HomeSectionType.Testimonial && x.IsActive),
            BranchCount = await _context.Branches.AsNoTracking().CountAsync(x => x.IsActive)
        };
    }
}
