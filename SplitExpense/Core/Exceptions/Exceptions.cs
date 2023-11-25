using System.Text.Json.Serialization;

namespace SplitExpense.Core.Exceptions
{
    public class BadRequestException : BaseException
    {
        public IList<string>? Errors { get; set; }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, IList<string> errors) : base(message)
        {
            this.Errors = errors;
        }
    }

    public class UnAuthorizedException : BaseException
    {
        public UnAuthorizedException(string message = "UnAuthorized user") : base(message)
        {

        }
    }

    public class ForbiddenException : BaseException
    {
        public ForbiddenException(string message = "Forbidden access") : base(message)
        {

        }
    }

    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string message = "Something went wrong") : base(message)
        {
        }
    }

    public class ExceptionResult
    {
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string>? Errors { get; set; }

        public ExceptionResult()
        {
            this.Message = string.Empty;
        }
    }
}
