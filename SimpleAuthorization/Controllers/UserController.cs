using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthorization.Models;
using SimpleAuthorization.Service.Interface;
using System.Security.Claims;

namespace SimpleAuthorization.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;
        
        public UserController(IUserService userService, IJwtTokenService jwtTokenService,
            IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();

            var user = await _userService.GetUserByEmailAsync(email);
            return user == null ? RedirectToAction("AllUsers") : View(user);
        }

        public async Task<IActionResult> AllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return View(users);
        }
        public async Task<IActionResult> Registration(UserRegistration user)
        {
            if (await _userService.RegisterUserAsync(user))
                return RedirectToAction("AllUsers");

            return View(user);
        }

        [HttpGet]
        public IActionResult Authorization()=> View();

        [HttpPost]
        public async Task<IActionResult> Authorization(UserAuthorization user)
        {
            var existingUser = await _userService.GetUserByEmailAsync(user.Login);

            if (existingUser != null && _passwordHasher.VerifyPassword(user.Password, existingUser.PasswordHash))
            {
                var token = _jwtTokenService.GenerateToken(existingUser.Email, "user");

                Response.Cookies.Append("jwt_token", token, new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });
                return RedirectToAction("Index");
            }
            return Unauthorized("Invalid password");
        }
    }
}
