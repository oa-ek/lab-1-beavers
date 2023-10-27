using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaverGame.Controllers;

public sealed partial class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<User> _usersRepository;
    private readonly IRepository<UserRole> _userRolesRepository;

    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    [GeneratedRegex(RegexPatterns.EmailPattern)]
    private static partial Regex EmailRegex();
    
    public UserController(IRepository<User> usersRepository, IRepository<UserRole> userRolesRepository, ILogger<HomeController> logger)
    {
        _usersRepository = usersRepository;
        _userRolesRepository = userRolesRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _usersRepository.GetAllEntitiesAsync());
    
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    public async Task<IActionResult> Update(Guid id)
    {
        PopulateDropdowns();

        var user = await _usersRepository.GetEntityByIdAsync(id);
        var dto = new UserDto()
        {
            UserId = user.Id.ToString(),
            Username = user.UserName,
            Email = user.Email,
        };
        return View(dto);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _usersRepository.GetEntityByIdAsync(id);
        
        return View(new UserDto()
        {
            UserId = user.Id.ToString(),
            Username = user.UserName,
            Email = user.Email
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDto dto)
    {
        if (!EmailRegex().IsMatch(dto.Email))
        {
            PopulateDropdowns();
            return View(dto);
        }

        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email
        };

        var passwordHasher = new PasswordHasher<User>();

        user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);
        
        await _usersRepository.AddNewEntityAsync(user);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserDto dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        var user = await _usersRepository.GetEntityByIdAsync(Guid.Parse(dto.UserId));
        if (user is null) 
            return NotFound();
        
        user.UserName = dto.Username;
        user.Email = dto.Email;
        
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

    private void PopulateDropdowns()
    {
        ViewData["UserRoleId"] = new SelectList(
            _userRolesRepository.GetAllEntities(), 
            nameof(UserRole.Id),
            nameof(UserRole.Name));

        ViewData["Users"] = new SelectList(
            _usersRepository.GetAllEntities(),
            nameof(Core.User.Id),
            nameof(Core.User.UserName));
    }
}