using SplitExpense.Core.Exceptions;

namespace SplitExpense.Core.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                int statusCode = 500;
                BaseException? exception = null;

                switch (ex)
                {
                    case BadRequestException:
                        statusCode = 400;
                        exception = (BadRequestException)ex;
                        break;
                    case UnAuthorizedException:
                        statusCode = 401;
                        exception = (UnAuthorizedException)ex;
                        break;
                    case ForbiddenException:
                        statusCode = 403;
                        exception = (ForbiddenException)ex;
                        break;
                    case InternalServerErrorException:
                        statusCode = 500;
                        exception = (InternalServerErrorException)ex;
                        break;
                }

                if (exception != null)
                {
                    var exceptionResult = new ExceptionResult()
                    {
                        Message = exception.Message
                    };

                    if (exception is BadRequestException)
                    {
                        exceptionResult.Errors = ((BadRequestException)exception).Errors;
                    }

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(exceptionResult));
                }
            }
        }
    }
}
