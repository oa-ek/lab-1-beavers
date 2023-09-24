using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed partial class ScreenshotController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Screenshot> _screenshotsRepository;
    private readonly IRepository<Game> _gamesRepository;

    [GeneratedRegex(RegexPatterns.UrlPattern)]
    private static partial Regex UrlRegex();
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    public ScreenshotController(IRepository<Screenshot> screenshotsRepository, IRepository<Game> _gamesRepository, ILogger<HomeController> logger)
    {
        _screenshotsRepository = screenshotsRepository;
        this._gamesRepository = _gamesRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _screenshotsRepository.GetAllEntitiesAsync());
    
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    public async Task<IActionResult> Update(Guid id)
    {
        PopulateDropdowns();
        var sreenshot = await _screenshotsRepository.GetEntityByIdAsync(id);
        var dto = new ScreenshotDto()
        {
            GameId = sreenshot.GameId.ToString(),
            ImageUrl = sreenshot.ImageUrl,
            ScreenshotId = sreenshot.ScreenshotId.ToString(),
        };
        return View(dto);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var sreenshot = await _screenshotsRepository.GetEntityByIdAsync(id);
        var dto = new ScreenshotDto()
        {
            GameId = sreenshot.GameId.ToString(),
            ImageUrl = sreenshot.ImageUrl,
            ScreenshotId = sreenshot.ScreenshotId.ToString(),
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ScreenshotDto dto)
    {
        if (!UrlRegex().IsMatch(dto.ImageUrl) || !GuidRegex().IsMatch(dto.GameId))
            return View(dto);
        
        await _screenshotsRepository.AddNewEntityAsync(new Screenshot
        {
            GameId = Guid.Parse(dto.GameId),
            ImageUrl = dto.ImageUrl,
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(ScreenshotDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var screenshot = await _screenshotsRepository.GetEntityByIdAsync(Guid.Parse(dto.ScreenshotId));
        if (screenshot is null) 
            return NotFound();

        screenshot.GameId = Guid.Parse(dto.GameId);
        screenshot.ImageUrl = dto.ImageUrl;
        _screenshotsRepository.UpdateExistingEntity(screenshot); 
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(ScreenshotDto dto)
    {
        if (!Guid.TryParse(dto.ScreenshotId, out var id)) return View(dto);
        
        var screenshot = await _screenshotsRepository.GetEntityByIdAsync(id);
        if (screenshot is null) 
            return NotFound();
        
        _screenshotsRepository.RemoveExistingEntity(screenshot); 
        return RedirectToAction("Index");
    }

    private void PopulateDropdowns()
    {
        ViewData["Games"] = new SelectList(
            _gamesRepository.GetAllEntities(),
            nameof(Game.GameId),
            nameof(Game.Name));
    }
}