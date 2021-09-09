using Microsoft.EntityFrameworkCore;

namespace ToDo.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}