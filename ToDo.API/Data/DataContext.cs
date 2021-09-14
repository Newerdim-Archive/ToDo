using Microsoft.EntityFrameworkCore;
using ToDo.API.Entities;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ToDo.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; init; }
    }
}