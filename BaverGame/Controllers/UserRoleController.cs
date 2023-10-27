using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed class UserRoleController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<UserRole> _rolesRepository;
    
    public UserRoleController(IRepository<UserRole> rolesRepository, ILogger<HomeController> logger)
    {
        _rolesRepository = rolesRepository;
        _logger = logger;
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
        
        await _rolesRepository.AddNewEntityAsync(new UserRole
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
        _rolesRepository.UpdateExistingEntity(role); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UserRoleDto dto)
    {
        if (!Guid.TryParse(dto.RoleId, out var id)) return View(dto);
        
        var role = await _rolesRepository.GetEntityByIdAsync(id);
        if (role is null) 
            return NotFound();
        
        _rolesRepository.RemoveExistingEntity(role); 
        return RedirectToAction("Index");
    }
}