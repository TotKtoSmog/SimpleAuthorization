using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleAuthorization.Models;

namespace SimpleAuthorization.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context) => _context = context;
        public async Task<IActionResult> Index()
        {
            List<User> Users = await _context.Users.ToListAsync();
            return View(Users);
        }

        public async Task<IActionResult> Registration(UserRegistration user)
        {
            if(user.Email != null && user.Password != null && user.Password == user.RepeatPassword)
            {

                var u = new User
                {
                    Fname = user.Fname,
                    Lname = user.Lname,
                    City = user.City,
                    Email = user.Email,
                    Phone = user.Phone,
                    PasswordHash = HashPassword(user.Password)
                };
                _context.Users.Add(u);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(user);
        }

        public string HashPassword(string password) 
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string password, string hashedPassword) 
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
