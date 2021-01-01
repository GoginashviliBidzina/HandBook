namespace HandBook.Api.Middlewares.Models
{
    public class ExceptionDetails
    {
        public int StatusCode { get; private set; }

        public string Message { get; private set; }

        public ExceptionDetails(int statusCode,
                                string message)
        {
            StatusCode = statusCode;
            Message = message;  
        }
    }
}
