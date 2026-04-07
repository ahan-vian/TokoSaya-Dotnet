
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Models;
using TokoSaya.ViewModels;
using TokoSaya.Utility;
using Microsoft.AspNetCore.Authorization;
namespace TokoSaya.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        var user = new ApplicationUser
        {
            Name = register.Name,
            Email = register.Email,
            UserName = register.Email
        };
        var result = await _userManager.CreateAsync(user, register.Password);
        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
            }
            await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View(register);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }
        var result = await _signInManager.PasswordSignInAsync(
            login.Email,
            login.Password,
            login.RememberMe,
            lockoutOnFailure: false
        );
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError("Error", "Email atau Password Salah!");
        return View(login);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        var model = new ProfileVM
        {
            Name = user.Name,
            Email = user.Email ?? ""
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        if (user.Name != model.Name)
        {
            user.Name = model.Name;
            await _userManager.UpdateAsync(user);
        }
        if (user.Email != model.Email)
        {
            await _userManager.SetEmailAsync(user, model.Email);
            await _userManager.SetUserNameAsync(user, model.Email);
        }
        if (!string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
        {
            var passwordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            await _signInManager.RefreshSignInAsync(user);
        }
        TempData["Success"] = "Profil Anda berhasil diperbarui!";
        return RedirectToAction("Profile");
    }
}