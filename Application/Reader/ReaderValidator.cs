using FluentValidation;
public class ReaderValidator : AbstractValidator<CreateReaderRequest>
{
    public ReaderValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя читателя обязательно")
            .MaximumLength(30).WithMessage("Имя читателя не должно превышать 30 символов");

        RuleFor(x => x.Phone)
            .MaximumLength(15).WithMessage("Номер телефона не должен превышать 15 символов");
    }
}