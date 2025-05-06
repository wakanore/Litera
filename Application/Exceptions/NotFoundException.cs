namespace Application.Exceptions
{
    public class NotFoundException : BaseApplicationException
    {
        public NotFoundException(string entityName, object key)
            : base(
                $"Сущность '{entityName}' с идентификатором '{key}' не найдена.",
                404,
                "Not Found")
        { }
    }
}