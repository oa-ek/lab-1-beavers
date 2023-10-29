using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public class DeveloperController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Developer> _developersRepository;
    
    public DeveloperController(IRepository<Developer> developersRepository, ILogger<HomeController> logger)
    {
        _developersRepository = developersRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => View(await _developersRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public async Task<IActionResult> Update(Guid id)
    {
        var dev = await _developersRepository.GetEntityByIdAsync(id);
        
        return View(new DeveloperDto
        {
            DeveloperId = id.ToString(),
            DeveloperName = dev.DeveloperName
        });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var dev = await _developersRepository.GetEntityByIdAsync(id);
        
        return View(new DeveloperDto
        {
            DeveloperId = id.ToString(),
            DeveloperName = dev.DeveloperName
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(DeveloperDto developer)
    {
        if (string.IsNullOrWhiteSpace(developer.DeveloperName) 
            || string.IsNullOrEmpty(developer.DeveloperName))
            return View(developer);
        
        await _developersRepository.AddNewEntityAsync(new Developer()
        {
            DeveloperName = developer.DeveloperName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(DeveloperDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        var developer = await _developersRepository.GetEntityByIdAsync(Guid.Parse(dto.DeveloperId));

        if (developer == null) return NotFound();
        
        developer.DeveloperName = dto.DeveloperName;
        
        _developersRepository.UpdateExistingEntity(developer); 

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeveloperDto dto)
    {
        if (!Guid.TryParse(dto.DeveloperId, out var id)) return View(dto);
        
        var developer = await _developersRepository.GetEntityByIdAsync(id);
        
        if (developer == null) return NotFound();
        
        _developersRepository.RemoveExistingEntity(developer); 
        
        return RedirectToAction("Index");
    }
}