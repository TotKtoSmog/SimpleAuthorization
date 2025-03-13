using Microsoft.AspNetCore.Mvc;
using SimpleAuthorization.Models;

namespace SimpleAuthorization.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context) => _context = context;
        public IActionResult Index()
        {
            List<User> Users = _context.Users.ToList();
            return View(Users);
        }

        public string HashPassword(string password) 
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string password, string hashedPassword) 
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
