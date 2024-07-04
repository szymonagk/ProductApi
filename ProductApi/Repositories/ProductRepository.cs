using Microsoft.EntityFrameworkCore;
using ProductApi.Entities;
using ProductApi.Interfaces;

namespace ProductApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext dbContext)
        {
            _context = dbContext;
        }

        public List<Product> GetAll()
        {
            return _context.Products.Include(p => p.Variants).Include(p => p.Images).ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.Include(p => p.Variants).Include(p => p.Images).FirstOrDefault(p => p.Id == id);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
    }
}
