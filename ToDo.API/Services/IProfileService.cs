using System.Threading.Tasks;
using ToDo.API.Dto;

namespace ToDo.API.Services
{
    public interface IProfileService
    {
        /// <summary>
        ///     Get profile by user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Profile if exists; otherwise, null</returns>
        public Task<Profile> GetByUserIdAsync(int userId);
    }
}