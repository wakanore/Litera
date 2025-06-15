using FluentValidation;

public class CreateFavouriteRequestValidator : AbstractValidator<CreateFavouriteRequest>
{
    public CreateFavouriteRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("ID пользователя обязательно")
            .GreaterThan(0).WithMessage("ID пользователя должен быть положительным");

        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("ID книги обязательно")
            .GreaterThan(0).WithMessage("ID книги должен быть положительным");

    }
}