using Test.Entities.Employee;
using Test.WebApi.Core.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.WebApi.Validators.Employee
{
    public class ResignationValidator : BaseValidator<ResignationRequest>
    {
        public ResignationValidator()
        {
            // RuleFor(x => x.EmployeeId).NotNull().WithMessage("Field is required");
            RuleFor(x => x.Proposeddate).NotEmpty().WithMessage("Field is required");
            
        }
    }
}
