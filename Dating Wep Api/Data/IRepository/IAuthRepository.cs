using Dating_Wep_Api.Models;

namespace Dating_Wep_Api.Data.IRepository
{
    public interface IAuthRepository
    {
        Task<User> RegisterUser(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExist(string username);
    }
}
