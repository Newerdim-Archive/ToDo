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
    public class ToDosController : CustomControllerBase
    {
        private readonly IUserService _userService;
        private readonly IToDoService _toDoService;

        public ToDosController(IUserService userService, IToDoService toDoService)
        {
            _userService = userService;
            _toDoService = toDoService;
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(CreateToDoModel model)
        {
            var currentUserId = _userService.GetCurrentId();
            
            var toDoToCreate = new ToDoToCreate
            {
                Title = model.Title,
                Description = model.Description,
                Deadline = model.Deadline
            };

            var createdToDo = await _toDoService.CreateAsync(currentUserId, toDoToCreate);
            
            return Ok(ResponseMessage.CreatedToDoSuccessfully, createdToDo);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllUserTodosAsync()
        {
            var currentUserId = _userService.GetCurrentId();

            var toDos = await _toDoService.GetAllFromUserAsync(currentUserId);
            
            return Ok(ResponseMessage.GotAllUserToDosSuccessfully, toDos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdFromUser(int id)
        {
            var currentUserId = _userService.GetCurrentId();

            var toDo = await _toDoService.GetByIdFromUserAsync(currentUserId, id);

            if (toDo is null)
            {
                return NotFound(ResponseMessage.NotFound);
            }
            
            return Ok(ResponseMessage.GotToDoByIdFromUserSuccessfully, toDo);
        }
    }
}