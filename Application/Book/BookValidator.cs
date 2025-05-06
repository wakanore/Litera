using FluentValidation;
public class BookValidator : AbstractValidator<CreateBookRequest>
{
    public BookValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя автора обязательно")
            .MaximumLength(30).WithMessage("Имя автора не должно превышать 30 символов");
    }
}