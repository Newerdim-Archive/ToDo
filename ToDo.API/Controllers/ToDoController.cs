using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Const;
using ToDo.API.Dto;
using ToDo.API.Helpers;
using ToDo.API.Models;
using ToDo.API.Services;

namespace ToDo.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ToDoController : CustomControllerBase
    {
        private readonly IUserService _userService;
        private readonly IToDoService _toDoService;

        public ToDoController(IUserService userService, IToDoService toDoService)
        {
            _userService = userService;
            _toDoService = toDoService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(CreateToDoModel model)
        {
            var createdToDo = await _toDoService.CreateAsync(new CreateToDo
            {
                UserId = _userService.GetCurrentId(),
                Title = model.Title,
                Description = model.Description,
                Deadline = model.Deadline
            });
            
            return Ok(ResponseMessage.CreatedToDoSuccessfully, createdToDo);
        }
    }
}