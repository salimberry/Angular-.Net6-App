using FluentValidation;
using Fullstack.Api.Models;

namespace Fullstack.Api.Validation
{
 

    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(e => e.Name).NotEmpty().MinimumLength(2);
            RuleFor(e => e.Email).NotEmpty().EmailAddress();
            RuleFor(e=>e.Phone).NotEmpty().NotEmpty();
            RuleFor(e => e.Salary).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }

}
