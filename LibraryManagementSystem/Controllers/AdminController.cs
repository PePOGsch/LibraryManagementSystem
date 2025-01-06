using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Administrator")] // Dostęp tylko dla administratorów
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/ManageRoles
        public IActionResult ManageRoles()
        {
            var users = _userManager.Users.ToList(); // Pobierz listę użytkowników
            return View(users);
        }

        // POST: Admin/AssignAdminRole
        [HttpPost]
        public async Task<IActionResult> AssignAdminRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            // Sprawdź, czy rola Administrator istnieje, jeśli nie, utwórz ją
            if (!await _roleManager.RoleExistsAsync("Administrator"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            // Przypisz rolę Administrator użytkownikowi
            if (!await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                await _userManager.AddToRoleAsync(user, "Administrator");
            }

            return RedirectToAction(nameof(ManageRoles));
        }

        // POST: Admin/RemoveAdminRole
        [HttpPost]
        public async Task<IActionResult> RemoveAdminRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            // Usuń rolę Administrator użytkownikowi
            if (await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Administrator");
            }

            return RedirectToAction(nameof(ManageRoles));
        }
    }
}
