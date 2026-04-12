using Microsoft.EntityFrameworkCore;
using ConstructionERP.Models.Entities;

namespace ConstructionERP.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin753"); // Default admin password and hashed settings

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserID = 1,
                    Username = "Admin",
                    Email = "admin@gmail.com",
                    PasswordHash = hashedPassword,
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now

                }
            );
        }
    }
}
