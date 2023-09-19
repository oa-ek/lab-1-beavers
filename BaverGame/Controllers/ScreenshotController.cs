using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed class ScreenshotController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Screenshot> _screenshotsRepository;
    
    public ScreenshotController(IRepository<Screenshot> screenshotsRepository, ILogger<HomeController> logger)
    {
        _screenshotsRepository = screenshotsRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _screenshotsRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(ScreenshotDto dto)
    {
        await _screenshotsRepository.AddNewEntityAsync(new Screenshot
        {
            GameId = dto.GameId,
            ImageUrl = dto.ImageUrl,
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(ScreenshotDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var screenshot = await _screenshotsRepository.GetEntityByIdAsync(dto.ScreenshotId);
        if (screenshot is null) 
            return NotFound();
        
        if (!dto.GameId.ToString().IsNullOrEmpty())
            screenshot.GameId = dto.GameId;
        
        if (!dto.ImageUrl.IsNullOrEmpty())
            screenshot.ImageUrl = dto.ImageUrl;
        
        _screenshotsRepository.UpdateExistingEntity(screenshot); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ScreenshotDto dto)
    {
        var screenshot = await _screenshotsRepository.GetEntityByIdAsync(dto.ScreenshotId);
        if (screenshot is null) 
            return NotFound();
        
        _screenshotsRepository.RemoveExistingEntity(screenshot); 
        return RedirectToAction("Index");
    }
}