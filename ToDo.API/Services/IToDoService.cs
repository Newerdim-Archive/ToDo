using System.Threading.Tasks;
using ToDo.API.Dto;

namespace ToDo.API.Services
{
    public interface IToDoService
    {
        /// <summary>
        ///     Create to-do
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="toDoToCreate">To-do</param>
        /// <returns>Created to-do</returns>
        public Task<Dto.ToDo> CreateAsync(int userId, ToDoToCreate toDoToCreate);
    }
}