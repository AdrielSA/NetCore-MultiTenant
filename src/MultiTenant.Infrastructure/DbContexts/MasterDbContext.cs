using Microsoft.EntityFrameworkCore;
using MultiTenant.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MultiTenant.Infrastructure.DbContexts
{
    public class MasterDbContext : DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(build =>
            {
                build.HasKey(x => x.Id);

                build.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(1, 1);

                build.HasOne(x => x.Organization)
                    .WithOne(e => e.User)
                    .HasPrincipalKey<User>(x => x.Id)
                    .HasForeignKey<Organization>(e => e.UserId);
            });

            modelBuilder.Entity<Organization>(build =>
            {
                build.HasKey(x => x.Id);

                build.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(1, 1);

                build.HasOne(x => x.User)
                    .WithOne(e => e.Organization)
                    .HasPrincipalKey<Organization>(x => x.Id)
                    .HasForeignKey<User>(e => e.OrganizationId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
