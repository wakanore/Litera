namespace Application.Exceptions
{
    public class NotFoundException : BaseApplicationException
    {
        public NotFoundException(string message)
            : base(message)
        { }
    }
}