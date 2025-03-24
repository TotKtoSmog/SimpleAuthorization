using Microsoft.EntityFrameworkCore;
using SimpleAuthorization.Models;
using SimpleAuthorization.Service.Interface;

namespace SimpleAuthorization.Service
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(ApplicationDbContext context, IPasswordHasher passwordHasher ) 
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<List<User>> GetAllUsersAsync()
            => await _context.Users.ToListAsync();
        public async Task<User?> GetUserByEmailAsync(string email)
            => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        public async Task<bool> RegisterUserAsync(UserRegistration user)
        {
            if (user.Email != null && user.Password != null && user.Password == user.RepeatPassword)
            {
                var newUser = new User
                {
                    Fname = user.Fname,
                    Lname = user.Lname,
                    City = user.City,
                    Email = user.Email,
                    Phone = user.Phone,
                    PasswordHash = _passwordHasher.HashPassword(user.Password)
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
