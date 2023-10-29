using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public sealed class UserRoleController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<UserRole> _rolesRepository;
    private readonly RoleManager<UserRole> _roleManager;
    
    public UserRoleController(IRepository<UserRole> rolesRepository, ILogger<HomeController> logger,
        RoleManager<UserRole> roleManager)
    {
        _rolesRepository = rolesRepository;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index() => 
        View(await _rolesRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public async Task<IActionResult> Update(Guid id)
    {
        var role = await _rolesRepository.GetEntityByIdAsync(id);
        
        return View(new UserRoleDto
        {
            RoleId = id.ToString(),
            RoleName = role.Name
        });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var role = await _rolesRepository.GetEntityByIdAsync(id);
        
        return View(new UserRoleDto
        {
            RoleId = id.ToString(),
            RoleName = role.Name
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RoleName) 
            || string.IsNullOrEmpty(dto.RoleName))
            return View(dto);
        
        await _roleManager.CreateAsync(new UserRole
        {
            Name = dto.RoleName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserRoleDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var role = await _rolesRepository.GetEntityByIdAsync(Guid.Parse(dto.RoleId));
        if (role is null) 
            return NotFound();
        
        role.Name = dto.RoleName;
        await _roleManager.UpdateAsync(role); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UserRoleDto dto)
    {
        if (!Guid.TryParse(dto.RoleId, out var id)) return View(dto);
        
        var role = await _rolesRepository.GetEntityByIdAsync(id);
        if (role is null) 
            return NotFound();
        
        await _roleManager.DeleteAsync(role); 
        return RedirectToAction("Index");
    }
}