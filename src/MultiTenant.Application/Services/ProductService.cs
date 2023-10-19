using MultiTenant.Domain.Contracts.IRepositories;
using MultiTenant.Domain.Contracts.IServices;
using MultiTenant.Domain.Entities;

namespace MultiTenant.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductUnitOfWork _productUnitOfWork;

        public ProductService(IProductUnitOfWork productUnitOfWork)
        {
            _productUnitOfWork = productUnitOfWork;
        }

        public Product GetProduct(int Id)
            => _productUnitOfWork.ProductRepository.GetById(Id);

        public List<Product> GetAllProducts()
            => _productUnitOfWork.ProductRepository.Get().ToList();

        public Product AddProduct(Product product)
        {
            var entity = _productUnitOfWork.ProductRepository.Add(product);
            _productUnitOfWork.SaveChanges();
            return entity;
        }

        public Product UpdateProduct(Product product)
        {
            var entity = GetProduct(product.Id);
            entity.Name = product.Name;
            entity.Description = product.Description;
            entity.Duration = product.Duration;
            _productUnitOfWork.ProductRepository.Update(entity);
            _productUnitOfWork.SaveChanges();

            return entity;
        }

        public void DeleteProduct(int id)
        {
            var entity = GetProduct(id);
            _productUnitOfWork.ProductRepository.Delete(entity);
            _productUnitOfWork.SaveChanges();
        }
    }
}
