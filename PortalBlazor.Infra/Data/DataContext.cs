using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PortalBlazor.Infra.Data
{
    public class DataContext : IdentityDbContext
    {
        //public DbSet<User> Users { get; set; }
        //public DbSet<AspNetUsers> AspNetUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase(databaseName: "portaldb");

            optionsBuilder.UseSqlServer("Data Source=sql.bsite.net\\MSSQL2016;Initial Catalog=danilosoad89_HOME;User ID=danilosoad89_HOME;Password=databasehome;Trust Server Certificate=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.ApplyConfiguration(new AspNetUsersMap());

            builder.Entity<IdentityRole>()
                    .HasData(new IdentityRole { Name = "User", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "USER" });

            builder.Entity<IdentityRole>()
                    .HasData(new IdentityRole { Name = "Admin", Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ADMIN" });
        }
    }
}