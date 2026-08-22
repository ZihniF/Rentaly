using AutoMapper;
using FluentValidation;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.EntityLayer.Entities;
using Rentaly.EntityLayer.Enums;
using Rentaly.BusinessLayer.Rules;

namespace Rentaly.BusinessLayer.Concrete
{
    public class RentalManager : IRentalService
    {
        private readonly IRentalDal _rentalDal;
        private readonly ICarDal _carDal;
        private readonly IBranchDal _branchDal;
        private readonly ICustomerDal _customerDal;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateRentalDto> _createValidator;
        private readonly IValidator<CreateBookingDto> _bookingValidator;
        private readonly IValidator<UpdateRentalDto> _updateValidator;
        private readonly IValidator<UpdateRentalStatusDto> _statusValidator;

        public RentalManager(
            IRentalDal rentalDal,
            ICarDal carDal,
            IBranchDal branchDal,
            ICustomerDal customerDal,
            IMapper mapper,
            IValidator<CreateRentalDto> createValidator,
            IValidator<CreateBookingDto> bookingValidator,
            IValidator<UpdateRentalDto> updateValidator,
            IValidator<UpdateRentalStatusDto> statusValidator)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _branchDal = branchDal;
            _customerDal = customerDal;
            _mapper = mapper;
            _createValidator = createValidator;
            _bookingValidator = bookingValidator;
            _updateValidator = updateValidator;
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

            await ValidateExistingRentalReferencesAsync(
                car, dto.CustomerId, dto.PickupBranchId, dto.ReturnBranchId);

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
            if (dto.PickupBranchId != car.BranchId)
                throw new InvalidOperationException("Araç yalnızca bulunduğu şubeden teslim alınabilir.");

            try
            {
                await _branchDal.GetByIdAsync(dto.ReturnBranchId);
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException("İade şubesi bulunamadı.");
            }

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

        public async Task TUpdateAsync(UpdateRentalDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);

            var car = await _carDal.GetByIdAsync(dto.CarId);
            if (!car.IsActive || !car.IsAvailable)
                throw new InvalidOperationException("Araç kiralamaya uygun değil.");

            await ValidateExistingRentalReferencesAsync(
                car, dto.CustomerId, dto.PickupBranchId, dto.ReturnBranchId);

            var rental = new Rental
            {
                RentalId = dto.RentalId,
                CarId = dto.CarId,
                CustomerId = dto.CustomerId,
                PickupBranchId = dto.PickupBranchId,
                ReturnBranchId = dto.ReturnBranchId,
                PickupDate = dto.PickupDate,
                ReturnDate = dto.ReturnDate,
                TotalPrice = car.DailyPrice * GetRentalDayCount(dto.PickupDate, dto.ReturnDate)
            };

            if (!await _rentalDal.TryUpdateRentalAsync(rental))
                throw new InvalidOperationException("Araç seçilen tarihler arasında müsait değil.");
        }

        private static int GetRentalDayCount(DateTime pickupDate, DateTime returnDate) =>
            Math.Max(1, (int)Math.Ceiling((returnDate - pickupDate).TotalDays));

        private async Task ValidateExistingRentalReferencesAsync(
            Car car, int customerId, int pickupBranchId, int returnBranchId)
        {
            if (pickupBranchId != car.BranchId)
                throw new InvalidOperationException("Araç yalnızca bulunduğu şubeden teslim alınabilir.");

            try
            {
                await _customerDal.GetByIdAsync(customerId);
                await _branchDal.GetByIdAsync(returnBranchId);
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException("Müşteri veya şube kaydı bulunamadı.");
            }
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

            if (!RentalStatusTransitionRules.CanTransition(rental.Status, dto.Status))
                throw new InvalidOperationException($"{rental.Status} durumundaki rezervasyon {dto.Status} durumuna geçirilemez.");

            rental.Status = dto.Status;

            await _rentalDal.UpdateAsync(rental);
        }

        public Task TDeleteAsync(int id) => _rentalDal.DeleteAsync(id);
    }
}
