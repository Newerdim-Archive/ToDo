using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.API.Dto;

namespace ToDo.API.Services
{
    public interface IToDoService
    {
        /// <summary>
        ///     Create to-do
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="toDoToCreate">To-do to create</param>
        /// <returns>Created to-do</returns>
        public Task<Dto.ToDo> CreateAsync(int userId, ToDoToCreate toDoToCreate);

        /// <summary>
        ///     Get all to-do's
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>To-do's</returns>
        public Task<ICollection<Dto.ToDo>> GetAllAsync(int userId);

        /// <summary>
        ///     Get to-do by id from user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="id">To-do ID</param>
        /// <returns>To-do if exists; otherwise, null</returns>
        public Task<Dto.ToDo> GetByIdAsync(int userId, int id);

        /// <summary>
        ///     Update existing to-do
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="toDoToUpdate">To-do to update</param>
        /// <returns>Updated to-do if exists; otherwise, null</returns>
        public Task<Dto.ToDo> UpdateAsync(int userId, ToDoToUpdate toDoToUpdate);
    }
}