using System;
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

            return Ok(ResponseMessage.ResourceCreatedSuccessfully, createdToDo);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var currentUserId = _userService.GetCurrentId();

            var toDos = await _toDoService.GetAllAsync(currentUserId);

            return Ok(ResponseMessage.ResourceGottenSuccessfully, toDos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var currentUserId = _userService.GetCurrentId();

            var toDo = await _toDoService.GetByIdAsync(currentUserId, id);

            if (toDo is null)
            {
                return NotFound(ResponseMessage.ResourceNotFound);
            }

            return Ok(ResponseMessage.ResourceGottenSuccessfully, toDo);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateToDoModel model)
        {
            var currentUserId = _userService.GetCurrentId();

            var toDoToUpdate = new ToDoToUpdate
            {
                Id = id,
                Title = model.Title,
                Description = model.Description,
                Deadline = model.Deadline,
                Completed = model.Completed
            };

            var updatedToDo = await _toDoService.UpdateAsync(currentUserId, toDoToUpdate);

            if (updatedToDo is null)
            {
                return NotFound(ResponseMessage.ResourceNotFound);
            }

            return Ok(ResponseMessage.ResourceUpdatedSuccessfully, updatedToDo);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var currentUserId = _userService.GetCurrentId();
            
            var isDeleted = await _toDoService.DeleteAsync(currentUserId, id);

            if (!isDeleted)
            {
                return NotFound(ResponseMessage.ResourceNotFound);
            }

            return Ok(ResponseMessage.ResourceDeletedSuccessfully);
        }
    }
}