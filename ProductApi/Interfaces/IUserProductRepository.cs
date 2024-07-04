using ProductApi.Entities;

namespace ProductApi.Interfaces
{
    public interface IUserProductRepository
    {
        UserProduct? GetByProductIdAndUserId(int productId, int userId);
        void Add(UserProduct userProduct);
        void Delete(UserProduct userProduct);
    }
}
