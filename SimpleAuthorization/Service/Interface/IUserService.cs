using SimpleAuthorization.Models;

namespace SimpleAuthorization.Service.Interface
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> RegisterUserAsync(UserRegistration user);
    }
}
