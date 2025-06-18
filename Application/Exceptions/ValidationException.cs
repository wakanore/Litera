using FluentValidation.Results;

namespace Application.Exceptions
{
    public class ValidationException : BaseApplicationException
    {
        public List<string> Errors { get; }  

        public ValidationException(List<ValidationFailure> failures)
            : base("Ошибка валидации", 400, "Validation Error")
        {
            Errors = failures
                .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                .ToList();
        }
    }
}