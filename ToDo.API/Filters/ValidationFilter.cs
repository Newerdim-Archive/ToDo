using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ToDo.API.Const;
using ToDo.API.Responses;

namespace ToDo.API.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                base.OnActionExecuting(context);
            }
            
            var errors = context.ModelState.Keys
                .Where(k => context.ModelState[k].Errors.Count > 0)
                .Select(k => new ValidationError
                {
                    Property = k,
                    Messages = context.ModelState[k].Errors
                        .Select(e => e.ErrorMessage)
                });

            var response = new ValidationErrorResponse 
            {
                Message = ResponseMessage.ValidationError,
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
}