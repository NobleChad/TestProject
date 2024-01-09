using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Data
{
    public class IDataRepository : IdentityDbContext<ApplicationUser>
    {
        public IDataRepository(DbContextOptions<IDataRepository> options)
            : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
    }
}