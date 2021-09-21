using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Dto;

namespace ToDo.API.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext _context;

        public ProfileService(DataContext context)
        {
            _context = context;
        }

        public async Task<Profile> GetByUserIdAsync(int userId)
        {
            var userInDb = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (userInDb is null)
            {
                return null;
            }

            return new Profile
            {
                Username = userInDb.Username,
                Email = userInDb.Email,
                ProfilePictureUrl = userInDb.ProfilePictureUrl
            };
        }
    }
}