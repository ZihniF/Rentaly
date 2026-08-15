using FluentValidation;
using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.BusinessLayer.ValidationRules;

public class UpdateRentalValidator : AbstractValidator<UpdateRentalDto>
{
    public UpdateRentalValidator()
    {
        RuleFor(x => x.RentalId).GreaterThan(0).WithMessage("Geçerli bir rezervasyon seçilmelidir.");
        RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Araç seçilmelidir.");
        RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Müşteri seçilmelidir.");
        RuleFor(x => x.PickupBranchId).GreaterThan(0).WithMessage("Teslim alma şubesi seçilmelidir.");
        RuleFor(x => x.ReturnBranchId).GreaterThan(0).WithMessage("İade şubesi seçilmelidir.");
        RuleFor(x => x.PickupDate).Must(date => date >= DateTime.Now.AddMinutes(-1))
            .WithMessage("Teslim alma tarihi geçmiş bir tarih olamaz.");
        RuleFor(x => x.ReturnDate).GreaterThan(x => x.PickupDate)
            .WithMessage("İade tarihi teslim alma tarihinden sonra olmalıdır.");
    }
}
