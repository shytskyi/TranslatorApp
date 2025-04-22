using BusinessLogicLayer.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Models.DTO;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private const int UserRole = 2; 
        private readonly ICRUDService<User> _crudService;
        private readonly IUserService _userService;

        public UserController(ICRUDService<User> crudService, IUserService userService)
        {
           _crudService = crudService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _crudService.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult ChangePassword(int id)
        {
            var model = new ChangePasswordViewModel
            {
                UserId = id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                bool ok = await _userService.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);

                if (!ok)
                {
                    ModelState.AddModelError("", "Failed to change password.");
                    return View(model);
                }

                //ViewBag.Message = "Successfully.";
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create() => View(new UserViewModel() { RoleId = UserRole});

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel user)
        {
            if (!ModelState.IsValid) return View(user);
            await _crudService.AddAsync(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _crudService.GetByIdAsync(id);
            UserViewModel viewUser = new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.RoleId
            };
            if (viewUser == null) return NotFound();
            return View(viewUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel user)
        {
            if (id != user.UserId) return BadRequest();
            if (!ModelState.IsValid) return View(user);

            await _crudService.UpdateAsync(id, user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _crudService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _crudService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}