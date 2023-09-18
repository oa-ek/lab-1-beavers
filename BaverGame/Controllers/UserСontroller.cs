using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<User> _usersRepository;
    
    public UserController(IRepository<User> usersRepository, ILogger<HomeController> logger)
    {
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _usersRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(UserDto dto)
    {
        await _usersRepository.AddNewEntityAsync(new User
        {
            Username = dto.Username,
            Email = dto.Email,
            UserRoleId = dto.RoleId,
            Password = dto.Password
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var user = await _usersRepository.GetEntityByIdAsync(dto.UserId);
        if (user is null) 
            return NotFound();
        
        if (!dto.RoleId.ToString().IsNullOrEmpty())
            user.UserRoleId = dto.RoleId;
        
        if (!dto.Username.IsNullOrEmpty())
            user.Username = dto.Username;
        
        if (!dto.Email.IsNullOrEmpty())
            user.Email = dto.Email;
        
        if (!dto.Password.IsNullOrEmpty())
            user.Password = dto.Password;
        
        _usersRepository.UpdateExistingEntity(user); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UserDto dto)
    {
        var user = await _usersRepository.GetEntityByIdAsync(dto.UserId);
        if (user is null) 
            return NotFound();
        
        _usersRepository.RemoveExistingEntity(user); 
        return RedirectToAction("Index");
    }
}