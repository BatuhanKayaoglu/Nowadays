using FluentValidation;
using Nowadays.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Validation
{
    public class CompanyValidations:AbstractValidator<AddCompanyViewModel>      
    {
        public CompanyValidations()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MinimumLength(5).WithMessage("5 karakterden az olamaz.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
        }   
    }
}
