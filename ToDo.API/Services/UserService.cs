using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Dto;
using ToDo.API.Enum;

namespace ToDo.API.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> GetByExternalIdAsync(string externalId, ExternalAuthProvider provider)
        {
            var userInDb = await _context.Users
                .AsNoTracking()
                .Where(u => u.Provider == provider)
                .FirstOrDefaultAsync(u => u.ExternalId == externalId);

            return _mapper.Map<User>(userInDb);
        }

        public async Task<bool> ExistsByExternalIdAsync(string externalId, ExternalAuthProvider provider)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Provider == provider)
                .AnyAsync(u => u.ExternalId == externalId);
        }

        public async Task<User> CreateAsync(CreateUser user)
        {
            var createdUser = new Entities.User
            {
                Username = user.Username,
                Email = user.Email,
                ExternalId = user.ExternalId,
                Provider = user.Provider,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            
            await _context.Users.AddAsync(createdUser);

            await _context.SaveChangesAsync();

            return _mapper.Map<User>(createdUser);
        }
    }
}