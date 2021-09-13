using System.Threading.Tasks;
using ToDo.API.Dto;
using ToDo.API.Enum;

namespace ToDo.API.Services
{
    public interface IUserService
    {
        /// <summary>
        ///     Get user by external id
        /// </summary>
        /// <param name="externalId">External id</param>
        /// <param name="provider">External provider</param>
        /// <returns>User if exists; otherwise, null</returns>
        public Task<User> GetByExternalIdAsync(string externalId, ExternalAuthProvider provider);
    }
}