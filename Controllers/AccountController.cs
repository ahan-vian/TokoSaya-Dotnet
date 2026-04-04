
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TokoSaya.Models;
using TokoSaya.ViewModels;

namespace TokoSaya.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
}