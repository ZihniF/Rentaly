using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IRentalService
    {
        Task<List<ResultRentalDto>> TGetListAsync();

        Task<GetRentalByIdDto?> TGetByIdAsync(int id);

        Task TCreateAsync(CreateRentalDto dto);

        Task<int> TCreateBookingAsync(CreateBookingDto dto);

        Task TUpdateStatusAsync(UpdateRentalStatusDto dto);

        Task TDeleteAsync(int id);
    }
}
