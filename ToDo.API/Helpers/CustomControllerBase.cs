using Microsoft.AspNetCore.Mvc;
using ToDo.API.Const;
using ToDo.API.Responses;

namespace ToDo.API.Helpers
{
    public class CustomControllerBase : ControllerBase
    {
        /// <summary>
        ///     Creates <see cref="OkObjectResult" /> with message
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Ok</returns>
        protected OkObjectResult Ok(string message)
        {
            return base.Ok(new BaseResponse
            {
                Message = message
            });
        }

        /// <summary>
        ///     Creates <see cref="OkObjectResult" /> with data and message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="data">Data</param>
        /// <returns>Ok</returns>
        protected OkObjectResult Ok<T>(string message, T data)
        {
            return base.Ok(new WithDataResponse<T>
            {
                Message = message,
                Data = data
            });
        }

        /// <summary>
        ///     Creates <see cref="ConflictObjectResult" /> with message
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Conflict</returns>
        protected ConflictObjectResult Conflict(string message)
        {
            return base.Conflict(new BaseResponse
            {
                Message = message
            });
        }

        /// <summary>
        ///     Creates <see cref="UnauthorizedObjectResult" /> with message
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Unauthorized</returns>
        protected UnauthorizedObjectResult Unauthorized(string message)
        {
            return base.Unauthorized(new BaseResponse
            {
                Message = message
            });
        }

        /// <summary>
        ///     Create <see cref="BadRequestObjectResult" /> with validation error
        /// </summary>
        /// <param name="invalidPropertyName">Name of invalid property</param>
        /// <param name="message">Validation error message from <see cref="ValidationErrorMessage" /></param>
        /// <returns>Bad request</returns>
        protected BadRequestObjectResult BadRequest(string invalidPropertyName, string message)
        {
            message = string.Format(message, invalidPropertyName);

            var errors = new[]
            {
                new ValidationError
                {
                    Property = invalidPropertyName,
                    Messages = new[] {message}
                }
            };

            return BadRequest(new ValidationErrorResponse
            {
                Message = ResponseMessage.ValidationError,
                Errors = errors
            });
        }

        /// <summary>
        ///     Creates <see cref="NotFoundObjectResult" /> with message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Not found</returns>
        protected NotFoundObjectResult NotFound(string message)
        {
            return base.NotFound(new BaseResponse
            {
                Message = message
            });
        }

        /// <summary>
        ///     Creates internal server error with message
        /// </summary>
        /// <param name="message">Error message</param>
        /// <returns>Internal server error</returns>
        protected InternalServerErrorObjectResult InternalServerError(string message)
        {
            var response = new BaseResponse
            {
                Message = message
            };

            return new InternalServerErrorObjectResult(response);
        }
    }
}