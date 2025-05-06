using System;

namespace Application.Exceptions
{
    public class BaseApplicationException : Exception
    {
        public int StatusCode { get; }  // HTTP-статус (400, 404, 500)
        public string Title { get; }    // Краткое описание ошибки

        public BaseApplicationException(
            string message,
            int statusCode = 400,
            string title = "Bad Request")
            : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }
    }
}