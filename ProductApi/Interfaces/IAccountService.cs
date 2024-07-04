using ProductApi.Models;

namespace ProductApi.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDTO registerUserDTO);
        string GenerateJwt(LoginDTO loginDTO);
    }
}
