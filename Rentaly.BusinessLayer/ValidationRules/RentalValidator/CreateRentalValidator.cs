using FluentValidation;
using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CreateRentalValidator
        : AbstractValidator<CreateRentalDto>
    {
        public CreateRentalValidator()
        {
            RuleFor(x => x.CarId)
                .GreaterThan(0)
                .WithMessage("Araç seçilmelidir.");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage("Müşteri seçilmelidir.");

            RuleFor(x => x.PickupBranchId)
                .GreaterThan(0)
                .WithMessage("Teslim alma şubesi seçilmelidir.");

            RuleFor(x => x.ReturnBranchId)
                .GreaterThan(0)
                .WithMessage("Teslim şubesi seçilmelidir.");

            RuleFor(x => x.PickupDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage(
                    "Teslim alma tarihi geçmiş bir tarih olamaz.");

            RuleFor(x => x.ReturnDate)
                .GreaterThan(x => x.PickupDate)
                .WithMessage(
                    "Teslim tarihi, teslim alma tarihinden sonra olmalıdır.");
        }
    }
}