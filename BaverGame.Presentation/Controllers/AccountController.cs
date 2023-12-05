using BaverGame.Application.DTOs.AuthenticationRelated;
using BaverGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    
    public AccountController(ILogger<HomeController> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public IActionResult SignIn()
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
    }

    public IActionResult SignUp()
    {
        if (User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ExecuteLogin(LoginFormDto loginData)
    {
        if (!ModelState.IsValid)
            return View("SignIn", loginData);

        var user = await _userManager.FindByNameAsync(loginData.InputUsername);

        if (user != null && await _userManager.CheckPasswordAsync(user, loginData.InputPassword))
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        if (!await _userManager.CheckPasswordAsync(user, loginData.InputPassword))
            ModelState.AddModelError(nameof(loginData.InputPassword), "Invalid password");
        
        return View("SignIn", loginData);
    }
    
    [HttpPost]
    public async Task<IActionResult> ExecuteRegistration(RegistrationFormDto model)
    {
        if (!ModelState.IsValid)
            return View("SignUp", model);
        
        var existingUser = await _userManager.FindByNameAsync(model.InputUsername);
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(model.InputUsername), "This username is already taken.");
            return View("SignUp", model);
        }

        existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
            return View("SignUp", model);
        }

        var newUser = new User
        {
            UserName = model.InputUsername,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(newUser, model.InputPassword);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, "User");
            return RedirectToAction("SignIn");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View("SignUp", model);
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}