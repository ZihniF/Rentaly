using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.WebUI.Services;

public interface IReservationEmailService
{
    Task<bool> SendApprovalAsync(ResultRentalDto rental, CancellationToken cancellationToken = default);
}
