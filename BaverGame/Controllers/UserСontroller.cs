using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed partial class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<User> _usersRepository;
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    [GeneratedRegex(RegexPatterns.EmailPattern)]
    private static partial Regex EmailRegex();
    
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
        if (!GuidRegex().IsMatch(dto.RoleId) || !EmailRegex().IsMatch(dto.Email))
            return View(dto);
        
        await _usersRepository.AddNewEntityAsync(new User
        {
            Username = dto.Username,
            Email = dto.Email,
            UserRoleId = Guid.Parse(dto.RoleId),
            Password = dto.Password
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var user = await _usersRepository.GetEntityByIdAsync(Guid.Parse(dto.UserId));
        if (user is null) 
            return NotFound();
        
        user.UserRoleId = Guid.Parse(dto.RoleId);
        user.Username = dto.Username;
        user.Email = dto.Email;
        user.Password = dto.Password;
        
        _usersRepository.UpdateExistingEntity(user); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UserDto dto)
    {
        if (!Guid.TryParse(dto.UserId, out var id)) return View(dto);
        
        var user = await _usersRepository.GetEntityByIdAsync(id);
        if (user is null) 
            return NotFound();
        
        _usersRepository.RemoveExistingEntity(user); 
        return RedirectToAction("Index");
    }
}