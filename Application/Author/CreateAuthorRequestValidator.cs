using FluentValidation;
public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя автора обязательно")
            .MaximumLength(30).WithMessage("Имя автора не должно превышать 30 символов");

        RuleFor(x => x.Phone)
            .MaximumLength(15).WithMessage("Номер телефона не должен превышать 15 символов");
    }
}