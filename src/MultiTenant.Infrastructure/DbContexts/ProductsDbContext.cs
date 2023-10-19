using Microsoft.EntityFrameworkCore;
using MultiTenant.Domain.Contracts.IServices;
using MultiTenant.Domain.Entities;

namespace MultiTenant.Infrastructure.DbContexts
{
    public class ProductsDbContext : DbContext
    {
        private readonly IProviderService _providerService;

        public ProductsDbContext(DbContextOptions<ProductsDbContext> options, IProviderService providerService) 
            : base(options)
        {
            _providerService = providerService;
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(build =>
            {
                build.HasKey(x => x.Id);

                build.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn(1, 1);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _providerService.GetConnetionString();

            optionsBuilder.UseSqlServer(connectionString, options => {
                options.MigrationsHistoryTable("__EFMigrationsHistory");
            });

            base.OnConfiguring(optionsBuilder);
        }
    }
}
