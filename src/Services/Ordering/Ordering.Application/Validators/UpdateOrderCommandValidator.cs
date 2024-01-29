using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class UpdateOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().NotNull().WithMessage("{UserName} is required")
            .MaximumLength(70).WithMessage("{UserName} most not exceed 70 characters");

        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required")
            .GreaterThanOrEqualTo(0).WithMessage("{TotalPrice} should be not negative");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required")
            .EmailAddress().WithMessage("{EmailAddress} is not correct");

        RuleFor(x => x.FirstName)
            .NotEmpty().NotNull().WithMessage("{FirstName} is required");
        
        RuleFor(x => x.LastName)
            .NotEmpty().NotNull().WithMessage("{LastName} is required");
    }
}