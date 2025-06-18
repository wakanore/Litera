using FluentValidation;

namespace Application.Validators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Style).NotEmpty();
            RuleFor(x => x.AuthorId).GreaterThan(0);
        }
    }
}