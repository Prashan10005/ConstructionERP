using Microsoft.EntityFrameworkCore;
using ConstructionERP.Models.Entities;

namespace ConstructionERP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
 
    }
}
