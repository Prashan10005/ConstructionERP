using FluentValidation;
using ConstructionERP.DTOs.User;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress().WithMessage("Valid email is required");

        RuleFor(x => x.Password)
            .NotEmpty().MinimumLength(8).WithMessage("Password must be at least 8 characters");

    }
}
