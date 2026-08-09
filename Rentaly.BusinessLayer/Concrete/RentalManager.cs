using AutoMapper;
using FluentValidation;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.EntityLayer.Entities;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.BusinessLayer.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalDal _rentalDal;
        private readonly ICarDal _carDal;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRentalDto> _createValidator;
        private readonly IValidator<UpdateRentalStatusDto> _statusValidator;

        public RentalManager(
            IRentalDal rentalDal,
            ICarDal carDal,
            IMapper mapper,
            IValidator<CreateRentalDto> createValidator,
            IValidator<UpdateRentalStatusDto> statusValidator)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _mapper = mapper;
            _createValidator = createValidator;
            _statusValidator = statusValidator;
        }

        public async Task<List<ResultRentalDto>> TGetListAsync()
        {
            var rentals =
                await _rentalDal.GetRentalsWithDetailsAsync();

            return _mapper.Map<List<ResultRentalDto>>(rentals);
        }

        public async Task<GetRentalByIdDto?> TGetByIdAsync(int id)
        {
            var rental =
                await _rentalDal.GetRentalWithDetailsByIdAsync(id);

            if (rental is null)
                return null;

            return _mapper.Map<GetRentalByIdDto>(rental);
        }

        public async Task TCreateAsync(CreateRentalDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);

            var car = await _carDal.GetByIdAsync(dto.CarId);

            if (car is null)
                throw new InvalidOperationException(
                    "Araç bulunamadı.");

            if (!car.IsActive)
                throw new InvalidOperationException(
                    "Araç aktif değil.");

            if (!car.IsAvailable)
                throw new InvalidOperationException(
                    "Araç kiralamaya uygun değil.");

            var hasConflict =
                await _rentalDal.HasDateConflictAsync(
                    dto.CarId,
                    dto.PickupDate,
                    dto.ReturnDate);

            if (hasConflict)
            {
                throw new InvalidOperationException(
                    "Araç seçilen tarihler arasında müsait değil.");
            }

            var rentalDayCount =
                (dto.ReturnDate.Date - dto.PickupDate.Date).Days;

            var rental = _mapper.Map<Rental>(dto);

            rental.TotalPrice =
                car.DailyPrice * rentalDayCount;

            rental.Status = RentalStatus.Pending;

            await _rentalDal.InsertAsync(rental);
        }

        public async Task TUpdateStatusAsync(
            UpdateRentalStatusDto dto)
        {
            await _statusValidator.ValidateAndThrowAsync(dto);

            var rental =
                await _rentalDal.GetByIdAsync(dto.RentalId);

            if (rental is null)
            {
                throw new InvalidOperationException(
                    "Rezervasyon bulunamadı.");
            }

            if (rental.Status != RentalStatus.Pending)
            {
                throw new InvalidOperationException(
                    "Yalnızca bekleyen rezervasyonlar " +
                    "onaylanabilir veya reddedilebilir.");
            }

            rental.Status = dto.Status;

            await _rentalDal.UpdateAsync(rental);
        }
    }
}