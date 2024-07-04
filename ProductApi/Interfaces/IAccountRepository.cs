using ProductApi.Entities;

namespace ProductApi.Interfaces
{
    public interface IAccountRepository
    {
        void Add(User user);
        User? GetByUsername(string username);
    }
}
