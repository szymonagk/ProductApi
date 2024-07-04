using Microsoft.EntityFrameworkCore;
using ProductApi.Entities;
using ProductApi.Interfaces;

namespace ProductApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ProductDbContext _context;
        public AccountRepository(ProductDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User? GetByUsername(string username)
        {
            return _context.Users.Include(u => u.Role).FirstOrDefault(u => u.Username == username);
        }


    }
}
