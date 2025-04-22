using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Models.DTO;
using Domain;

namespace PresentationLayer.Controllers
{
    public class AuthorizationController : Controller
    {
        private const int UserRole = 2;
        private readonly IUserService _userService;
        private readonly ICRUDService<User> _createUserService;
        public AuthorizationController(IUserService userService, ICRUDService<User> createUserService)
        {
            _userService = userService;
            _createUserService = createUserService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registration() => View(new UserViewModel() { RoleId = UserRole });

        [HttpPost]
        public async Task<IActionResult> Registration(UserViewModel user)
        {
            if (!ModelState.IsValid) return View(user);
            await _createUserService.AddAsync(user);
            var userLogin = new LoginViewModel
            {
                Email = user.Email,
                Password = user.Password,
                RememberMe = false
            };
            return await Login(userLogin);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.ValidateUser(model.Email, model.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Incorrect Email or password.");
                return View(model);
            }
            

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user!.Email),
                new Claim(ClaimTypes.Role, user!.RoleName)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)//AddDays(1)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Login", "Authorization");          
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}
