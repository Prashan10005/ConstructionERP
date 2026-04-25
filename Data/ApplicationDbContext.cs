using Microsoft.EntityFrameworkCore;
using ConstructionERP.Models.Entities;

namespace ConstructionERP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> ProjectTasks { get; set; }
        public DbSet<MaterialRequest> MaterialRequest { get; set; }

    }
}
