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
        private readonly IValidator<CreateBookingDto> _bookingValidator;
        private readonly IValidator<UpdateRentalStatusDto> _statusValidator;

        public RentalManager(
            IRentalDal rentalDal,
            ICarDal carDal,
            IMapper mapper,
            IValidator<CreateRentalDto> createValidator,
            IValidator<CreateBookingDto> bookingValidator,
            IValidator<UpdateRentalStatusDto> statusValidator)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _mapper = mapper;
            _createValidator = createValidator;
            _bookingValidator = bookingValidator;
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
            {
                throw new InvalidOperationException(
                    "Araç bulunamadı.");
            }

            if (!car.IsActive)
            {
                throw new InvalidOperationException(
                    "Araç aktif değil.");
            }

            if (!car.IsAvailable)
            {
                throw new InvalidOperationException(
                    "Araç kiralamaya uygun değil.");
            }

            var rentalDayCount = GetRentalDayCount(dto.PickupDate, dto.ReturnDate);

            var rental = _mapper.Map<Rental>(dto);

            rental.TotalPrice =
                car.DailyPrice * rentalDayCount;

            rental.Status = RentalStatus.Pending;

            var isCreated =
                await _rentalDal.TryCreateRentalAsync(rental);

            if (!isCreated)
            {
                throw new InvalidOperationException(
                    "Araç seçilen tarihler arasında müsait değil.");
            }
        }

        public async Task<int> TCreateBookingAsync(CreateBookingDto dto)
        {
            await _bookingValidator.ValidateAndThrowAsync(dto);

            var car = await _carDal.GetByIdAsync(dto.CarId);
            if (car is null) throw new InvalidOperationException("Araç bulunamadı.");
            if (!car.IsActive || !car.IsAvailable)
                throw new InvalidOperationException("Araç kiralamaya uygun değil.");

            var customer = new Customer
            {
                Name = dto.Name.Trim(),
                Surname = dto.Surname.Trim(),
                Email = dto.Email.Trim(),
                Phone = dto.Phone.Trim(),
                IdentityNumber = dto.IdentityNumber?.Trim() ?? string.Empty,
                DrivingLicenseNumber = string.Empty,
                DrivingLicenseDate = DateTime.Today
            };
            var rental = new Rental
            {
                CarId = dto.CarId,
                PickupBranchId = dto.PickupBranchId,
                ReturnBranchId = dto.ReturnBranchId,
                PickupDate = dto.PickupDate,
                ReturnDate = dto.ReturnDate,
                TotalPrice = car.DailyPrice * GetRentalDayCount(dto.PickupDate, dto.ReturnDate),
                Status = RentalStatus.Pending
            };

            if (!await _rentalDal.TryCreateBookingAsync(customer, rental))
                throw new InvalidOperationException("Araç seçilen tarihler arasında müsait değil.");

            return rental.RentalId;
        }

        private static int GetRentalDayCount(DateTime pickupDate, DateTime returnDate) =>
            Math.Max(1, (int)Math.Ceiling((returnDate - pickupDate).TotalDays));

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

        public Task TDeleteAsync(int id) => _rentalDal.DeleteAsync(id);
    }
}
