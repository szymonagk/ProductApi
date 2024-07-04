using ProductApi.Entities;

namespace ProductApi.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? GetById(int id);
        void Update(Product product);
    }
}
