using FluentValidation;


public sealed record CreateFavouriteRequest(
    int UserId,
    int BookId
);

public sealed record UpdateFavouriteRequest(
    int UserId,
    int BookId
);


public class CreateFavouriteRequestValidator : AbstractValidator<CreateFavouriteRequest>
{
    public CreateFavouriteRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.BookId).NotEmpty().GreaterThan(0);
    }
}

public class UpdateFavouriteRequestValidator : AbstractValidator<UpdateFavouriteRequest>
{
    public UpdateFavouriteRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.BookId).NotEmpty().GreaterThan(0);
    }
}