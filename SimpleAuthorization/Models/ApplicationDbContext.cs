using Microsoft.EntityFrameworkCore;

namespace SimpleAuthorization.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }
        ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property("Id").HasColumnName("id");
                entity.Property("Fname").HasColumnName("fname");
                entity.Property("Lname").HasColumnName("lname");
                entity.Property("City").HasColumnName("city");
                entity.Property("Phone").HasColumnName("phone");
                entity.Property("Email").HasColumnName("email");
                entity.Property("PasswordHash").HasColumnName("password_hash");
            });
        }
    }
}
