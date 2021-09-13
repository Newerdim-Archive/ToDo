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

        /// <summary>
        ///     Check if the user exists
        /// </summary>
        /// <param name="externalId">External id</param>
        /// <param name="provider">External provider</param>
        /// <returns>True if exists; otherwise, null</returns>
        public Task<bool> ExistsByExternalIdAsync(string externalId, ExternalAuthProvider provider);

        /// <summary>
        ///     Create new user
        /// </summary>
        /// <param name="user">User to create</param>
        /// <returns>Newly created user</returns>
        public Task<User> CreateAsync(CreateUser user);
    }
}