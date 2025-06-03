using System;

namespace Application.Exceptions
{
    public class BaseApplicationException : Exception
    {
        public int StatusCode { get; }  
        public string Title { get; }    

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