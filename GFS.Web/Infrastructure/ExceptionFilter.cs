using GFS.Core;
using GFS.Core.Enums;
using GFS.Transfer.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GFS.Web.Infrastructure
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly Dictionary _dictionary;
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IOptions<Dictionary> dictionary)
        {
            _logger = logger;
            _dictionary = dictionary.Value;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(GfsException))
            {
                if (context.Exception is GfsException brightlyException)
                {
                    _logger?.LogError(
                        $"Recived known brightly exception, sending bad request. Code:{brightlyException.ErrorCode} Messages:{brightlyException.Message}. Stack:{brightlyException.StackTrace}.");

                    context.Result = new BadRequestObjectResult(new ResponseDto<object>
                    {
                        Error = new ErrorDto
                        {
                            ErrorCode = brightlyException.ErrorCode,
                            ErrorName= brightlyException.ErrorCode.ToString(),
                            Message = _dictionary.GetMessage(brightlyException.ErrorCode)
                        }
                    });
                }
            }
            else
            {
                _logger?.LogError(
                    $"Recived unknown brightly exception, sending bad request. Message:{context.Exception.Message}. Stack:{context.Exception.StackTrace}.");

                context.Result = new BadRequestObjectResult(new ResponseDto<object>
                {
                    Error = new ErrorDto
                    {
                        ErrorCode = ErrorCode.UnexpectedError,
                        Message = ErrorCode.UnexpectedError.ToString()
                    }
                });
            }
        }
    }
}