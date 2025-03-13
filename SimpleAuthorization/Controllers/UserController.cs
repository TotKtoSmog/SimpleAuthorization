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
    }
}
