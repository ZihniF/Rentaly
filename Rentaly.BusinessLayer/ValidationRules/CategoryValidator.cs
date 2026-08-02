using FluentValidation;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CategoryValidator: AbstractValidator<Category>
    {
        public CategoryValidator() 
        { 
            RuleFor(c => c.CategoryName).NotEmpty().WithMessage("Kategori adı boş bırakılamaz.");
            RuleFor(c => c.CategoryName).MinimumLength(2).WithMessage("Kategori adı en az 2 karakter " +
                "olmalıdır.").MaximumLength(30).WithMessage("Kategori adı en fazla 30 karakter olabilir.");

        }
    }
}
