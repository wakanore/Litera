using FluentValidation;

namespace Application.Validators
{
    public class UpdateReaderRequestValidator : AbstractValidator<UpdateReaderRequest>
    {
        public UpdateReaderRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}