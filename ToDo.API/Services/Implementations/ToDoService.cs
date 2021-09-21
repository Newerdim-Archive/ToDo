using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Dto;

namespace ToDo.API.Services.Implementations
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

        public async Task<Dto.ToDo> CreateAsync(int userId, ToDoToCreate toDoToCreate)
        {
            var toDo = new Entities.ToDo
            {
                UserId = userId,
                Title = toDoToCreate.Title,
                Description = toDoToCreate.Description,
                Deadline = toDoToCreate.Deadline,
                Completed = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();

            return _mapper.Map<Dto.ToDo>(toDo);
        }

        public async Task<ICollection<Dto.ToDo>> GetAllAsync(int userId)
        {
            var toDos = await _context.ToDos
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return _mapper.Map<ICollection<Dto.ToDo>>(toDos);
        }

        public async Task<Dto.ToDo> GetByIdAsync(int userId, int id)
        {
            var toDo = await _context.ToDos
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<Dto.ToDo>(toDo);
        }

        public async Task<Dto.ToDo> UpdateAsync(int userId, ToDoToUpdate toDoToUpdate)
        {
            var toDoFromDb = await _context.ToDos
                .Where(t => t.UserId == userId)
                .FirstOrDefaultAsync(t => t.Id == toDoToUpdate.Id);

            _mapper.Map(toDoToUpdate, toDoFromDb);

            await _context.SaveChangesAsync();

            return _mapper.Map<Dto.ToDo>(toDoFromDb);
        }

        public async Task<bool> DeleteAsync(int userId, int toDoId)
        {
            var toDoInDb = await _context.ToDos
                .Where(t => t.UserId == userId)
                .FirstOrDefaultAsync(t => t.Id == toDoId);

            if (toDoInDb is null)
            {
                return false;
            }

            _context.ToDos.Remove(toDoInDb);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}