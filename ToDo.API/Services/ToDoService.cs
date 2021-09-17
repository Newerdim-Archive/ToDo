using System;
using System.Threading.Tasks;
using AutoMapper;
using ToDo.API.Data;
using ToDo.API.Dto;

namespace ToDo.API.Services
{
    public class ToDoService : IToDoService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ToDoService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Dto.ToDo> CreateAsync(CreateToDo createToDo)
        {
            var toDo = new Entities.ToDo
            {
                UserId = createToDo.UserId,
                Title = createToDo.Title,
                Description = createToDo.Description,
                Deadline = createToDo.Deadline,
                Completed = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();

            return _mapper.Map<Dto.ToDo>(toDo);
        }
    }
}