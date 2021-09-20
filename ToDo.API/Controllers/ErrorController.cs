using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Const;
using ToDo.API.Helpers;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ErrorController : CustomControllerBase
    {
        [AllowAnonymous]
        public IActionResult Error()
        {
            return InternalServerError(ResponseMessage.UnexpectedError);
        }
    }
}