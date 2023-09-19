using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed partial class GameController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Game> _gamesRepository;
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    public GameController(IRepository<Game> gamesRepository, ILogger<HomeController> logger)
    {
        _gamesRepository = gamesRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _gamesRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(GameDto dto)
    {
        if (!GuidRegex().IsMatch(dto.DeveloperId) || !GuidRegex().IsMatch(dto.PublisherId)
                                                  || string.IsNullOrWhiteSpace(dto.Name)
                                                  || string.IsNullOrWhiteSpace(dto.Description)
                                                  || string.IsNullOrWhiteSpace(dto.SystemRequirements))
            return View(dto);
        
        await _gamesRepository.AddNewEntityAsync(new Game
        {
            DeveloperId = Guid.Parse(dto.DeveloperId),
            PublisherId = Guid.Parse(dto.PublisherId),
            Name = dto.Name,
            Description = dto.Description,
            SystemRequirements = dto.SystemRequirements
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(GameDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var game = await _gamesRepository.GetEntityByIdAsync(Guid.Parse(dto.GameId));
        
        if (game is null) 
            return NotFound();
        
        game.DeveloperId = Guid.Parse(dto.DeveloperId);
        game.PublisherId = Guid.Parse(dto.PublisherId);
        game.Name = dto.Name;
        game.Description = dto.Description;
        game.SystemRequirements = dto.SystemRequirements;
        _gamesRepository.UpdateExistingEntity(game); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(GameDto dto)
    {
        if (!Guid.TryParse(dto.GameId, out var id)) return View(dto);
        
        var game = await _gamesRepository.GetEntityByIdAsync(id);
        if (game is null) 
            return NotFound();
        
        _gamesRepository.RemoveExistingEntity(game); 
        return RedirectToAction("Index");
    }
}