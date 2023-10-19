using Microsoft.EntityFrameworkCore;
using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Entities;
using MultiTenant.Infrastructure.DbContexts;

namespace MultiTenant.Infrastructure.Repositories
{
    public class ProductUnitOfWork : IProductUnitOfWork
    {
        private readonly ProductsDbContext _context;
        private bool _disposed;

        ~ProductUnitOfWork() => Dispose(false);
        public ProductUnitOfWork(ProductsDbContext context)
        {
            _context = context;
            ProductRepository = new Repository<Product>(_context);
        }

        public IRepository<Product> ProductRepository { get; private set; }

        public int SaveChanges() => _context.SaveChanges();

        public void MigrateDatabase()
        {
            _context.Database.Migrate();

            var script = _context.Database.GenerateCreateScript();
            script = script.Replace("GO", string.Empty);
            _context.Database.ExecuteSqlRaw(script);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing && _disposed)
                _context.Dispose();
            _disposed = true;
        }
    }
}
