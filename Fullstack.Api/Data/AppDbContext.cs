using Fullstack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> employees { get; set; }


    }
}
