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
    }
}