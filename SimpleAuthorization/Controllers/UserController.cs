using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleAuthorization.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleAuthorization.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        } 
        public async Task<IActionResult> Index(int id)
        {
            User? u = await _context.Users.FirstOrDefaultAsync(n => n.Id == id);
            if (id == 0 || u == null) return RedirectToAction("AllUsers");
            return View(u);
        }

        public async Task<IActionResult> AllUsers()
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
                return RedirectToAction("AllUsers");

            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Authorization()=> View();

        [HttpPost]
        public async Task<IActionResult> Authorization(UserAuthorization user)
        {
            User? u = await _context.Users.FirstOrDefaultAsync(n => n.Email == user.Login);

            if (u == null) return View(user);
            else if (VerifyPassword(user.Password, u.PasswordHash))
                return RedirectToAction("Index", new { id = u.Id });
            return View(user);

        }

        public string HashPassword(string password) 
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool VerifyPassword(string password, string hashedPassword) 
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    
        public string GenerateJwtToken(string username, string role)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpireMinutes"])),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
