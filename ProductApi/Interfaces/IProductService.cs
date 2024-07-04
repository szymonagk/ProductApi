using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAll();
        List<ProductWithUserFlagDTO> GetAllProductsWithUserFlag(int userId);
        Product GetById(int id);
        string AddOrUpdateFlaggedCountAndReturnMessage(ProductIdAndUserFlagDTO productDTO, int userId);
    }
}
