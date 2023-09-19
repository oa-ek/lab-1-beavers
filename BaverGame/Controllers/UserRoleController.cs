using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed class UserRoleController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<UserRole> _tagsRepository;
    
    public UserRoleController(IRepository<UserRole> tagsRepository, ILogger<HomeController> logger)
    {
        _tagsRepository = tagsRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _tagsRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(UserRoleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RoleName) 
            || string.IsNullOrEmpty(dto.RoleName))
            return View(dto);
        
        await _tagsRepository.AddNewEntityAsync(new UserRole
        {
            RoleName = dto.RoleName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserRoleDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var role = await _tagsRepository.GetEntityByIdAsync(Guid.Parse(dto.RoleId));
        if (role is null) 
            return NotFound();
        
        role.RoleName = dto.RoleName;
        _tagsRepository.UpdateExistingEntity(role); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UserRoleDto dto)
    {
        if (!Guid.TryParse(dto.RoleId, out var id)) return View(dto);
        
        var role = await _tagsRepository.GetEntityByIdAsync(id);
        if (role is null) 
            return NotFound();
        
        _tagsRepository.RemoveExistingEntity(role); 
        return RedirectToAction("Index");
    }
}