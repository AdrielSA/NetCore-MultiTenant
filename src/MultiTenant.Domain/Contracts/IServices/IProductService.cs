using MultiTenant.Domain.Entities;

namespace MultiTenant.Domain.Contracts.IServices
{
    public interface IProductService
    {
        Product AddProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetAllProducts();
        Product GetProduct(int Id);
        Product UpdateProduct(Product product);
    }
}