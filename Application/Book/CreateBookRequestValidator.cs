using FluentValidation;

namespace Application.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Style).NotEmpty();
            RuleFor(x => x.AuthorId).GreaterThan(0);
        }
    }
}