using ProductApi.Entities;
using ProductApi.Interfaces;

namespace ProductApi.Repositories
{
    public class UserProductRepository : IUserProductRepository
    {
        private readonly ProductDbContext _context;
        public UserProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public UserProduct? GetByProductIdAndUserId(int productId, int userId)
        {
            return _context.UsersProducts
                .FirstOrDefault(u => u.ProductId == productId && u.UserId == userId);
        }

        public void Add(UserProduct userProduct)
        {
            _context.UsersProducts.Add(userProduct);
            _context.SaveChanges();
        }

        public void Delete(UserProduct userProduct) 
        {
            _context.UsersProducts.Remove(userProduct);
            _context.SaveChanges();
        }

        public List<UserProduct> GetAllByUserId(int userId)
        {
            return _context.UsersProducts.Where(up => up.UserId == userId).ToList();
        }
    }
}
