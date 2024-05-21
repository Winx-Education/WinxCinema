using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Winx_Cinema.Shared.Entities;

namespace Winx_Cinema.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Film> Films { get; set; } = null!;
        public DbSet<FilmResolution> FilmResolutions { get; set; } = null!;
        public DbSet<Hall> Halls { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var ordinaryUserId = "e7df795b-97bd-40fe-b7bc-2a908b1a61e9";
            var adminUserId = "06b7173c-4e2e-4496-868b-811f09e34f14";

            var roles = new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = ordinaryUserId,
                    ConcurrencyStamp = ordinaryUserId,
                    Name = "ordinaryUser",
                    NormalizedName = "ordinaryUser".ToUpper()
                },
                new IdentityRole()
                {
                    Id=adminUserId,
                    ConcurrencyStamp=adminUserId,
                    Name = "adminUser",
                    NormalizedName = "adminUser".ToUpper()
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

        }

    }
}
