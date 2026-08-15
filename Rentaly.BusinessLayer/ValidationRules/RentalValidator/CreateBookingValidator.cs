using FluentValidation;
using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.BusinessLayer.ValidationRules;

public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
{
    public CreateBookingValidator()
    {
        RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Araç seçilmelidir.");
        RuleFor(x => x.PickupBranchId).GreaterThan(0).WithMessage("Teslim alma şubesi seçilmelidir.");
        RuleFor(x => x.ReturnBranchId).GreaterThan(0).WithMessage("İade şubesi seçilmelidir.");
        RuleFor(x => x.PickupDate).Must(date => date >= DateTime.Now.AddMinutes(-1))
            .WithMessage("Teslim alma tarihi geçmiş bir tarih olamaz.");
        RuleFor(x => x.ReturnDate).GreaterThan(x => x.PickupDate)
            .WithMessage("İade tarihi teslim alma tarihinden sonra olmalıdır.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(60).WithMessage("Ad zorunludur.");
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(60).WithMessage("Soyad zorunludur.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(30).WithMessage("Telefon zorunludur.");
        RuleFor(x => x.IdentityNumber).Matches("^[0-9]{11}$")
            .When(x => !string.IsNullOrWhiteSpace(x.IdentityNumber))
            .WithMessage("T.C. kimlik numarası 11 haneli olmalıdır.");
    }
}
