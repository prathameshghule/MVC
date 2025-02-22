using Microsoft.EntityFrameworkCore;
using VCSSYSTEMS.Models.Domain;

namespace VCSSYSTEMS.Data
{
    public class MVCDemoDbContext : DbContext
    {
        public MVCDemoDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
