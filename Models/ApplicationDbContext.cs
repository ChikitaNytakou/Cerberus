using ByeBye.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Miniproj.Coefficients;
using System.Reflection.Emit;

namespace ByeBye.Models
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Coefficient> Coefficients { get; set; }
        public DbSet<Polygon> Polygons { get; set; }
        public DbSet<SrVesGrPoezdaCoFour> SrVesGrPoezdaCoFour { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customizing the ASP.NET Identity model and overriding the defaults if needed
            builder.Entity<IdentityUserRole<string>>()
                   .HasOne<IdentityRole>()
                   .WithMany()
                   .HasForeignKey(ur => ur.RoleId)
                   .OnDelete(DeleteBehavior.NoAction);

            // Настройка для Polygon
            builder.Entity<Polygon>()
                .ToTable("Polygons")
                .HasKey(p => p.Id);

            builder.Entity<Polygon>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100); // Пример ограничения длины

            // Настройка для SrVesGrPoezdaCoFour
            builder.Entity<SrVesGrPoezdaCoFour>()
                .ToTable("SrVesGrPoezdaCoFour")
                .HasKey(s => s.Id);

            builder.Entity<SrVesGrPoezdaCoFour>()
                .HasOne(s => s.Polygon)
                .WithMany()
                .HasForeignKey(s => s.PolygonId);

            //The following code will set ON DELETE NO ACTION to all Foreign Keys
            //foreach (var foreignKey in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            //}
        }
    }
}