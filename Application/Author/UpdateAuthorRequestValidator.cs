using FluentValidation;

namespace Application.Validators
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}