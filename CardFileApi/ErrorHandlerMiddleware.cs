using BLL.Validation;
using CardFileApi.Logging;
using System.Net;
using System.Text.Json;

namespace CardFileApi
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ErrorHandlerMiddleware(RequestDelegate next,
            ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case CardFileException e:

                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case NotFoundException e:

                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    case UnauthorizedException e:

                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;

                    default:

                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogInfo(error?.Message);
                var result = JsonSerializer.Serialize(error?.Message);
                await response.WriteAsync(result);
            }
        }
    }
}
