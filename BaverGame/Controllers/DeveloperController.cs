using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public class DeveloperController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Developer> _developerRepository;
    
    public DeveloperController(IRepository<Developer> developerRepository, ILogger<HomeController> logger)
    {
        _developerRepository = developerRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => View(await _developerRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(DeveloperDto developer)
    {
        await _developerRepository.AddNewEntityAsync(new Developer()
        {
            DeveloperName = developer.DeveloperName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(DeveloperDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        var developer = await _developerRepository.GetEntityByIdAsync(dto.DeveloperId);

        if (developer == null) return NotFound();
        
        developer.DeveloperName = dto.DeveloperName;
        
        _developerRepository.UpdateExistingEntity(developer); 

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeveloperDto dto)
    {
        var developer = await _developerRepository.GetEntityByIdAsync(dto.DeveloperId);
        
        if (developer == null) return NotFound();
        
        _developerRepository.RemoveExistingEntity(developer); 
        
        return RedirectToAction("Index");
    }
}