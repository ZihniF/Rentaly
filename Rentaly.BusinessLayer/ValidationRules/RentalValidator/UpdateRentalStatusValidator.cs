using FluentValidation;
using Rentaly.DtoLayer.RentalDtos;
using Rentaly.EntityLayer.Enums;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class UpdateRentalStatusValidator
        : AbstractValidator<UpdateRentalStatusDto>
    {
        public UpdateRentalStatusValidator()
        {
            RuleFor(x => x.RentalId)
                .GreaterThan(0)
                .WithMessage(
                    "Geçerli bir rezervasyon seçilmelidir.");

            RuleFor(x => x.Status)
                .Must(status =>
                    status == RentalStatus.Approved ||
                    status == RentalStatus.Rejected)
                .WithMessage(
                    "Rezervasyon yalnızca onaylanabilir veya reddedilebilir.");
        }
    }
}