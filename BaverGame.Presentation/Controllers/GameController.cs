using System.Text.RegularExpressions;
using BaverGame.Application.DTOs;
using BaverGame.Application.DTOs.ValidationRelated;
using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public sealed partial class GameController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<Publisher> _publisherRepository;
    private readonly IRepository<Developer> _developersRepository;

    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    public GameController(IRepository<Game> gamesRepository, IRepository<Publisher> publisherRepository, IRepository<Developer> developersRepository, ILogger<HomeController> logger)
    {
        _gamesRepository = gamesRepository;
        _publisherRepository = publisherRepository;
        _developersRepository = developersRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() =>
        View(await _gamesRepository.GetAllEntitiesAsync());

    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    public async Task<IActionResult> Update(Guid id)
    {
        PopulateDropdowns();
        var game = await _gamesRepository.GetEntityByIdAsync(id);
        var dto = new GameDto
        {
            GameId = game.GameId.ToString(),
            Name = game.Name,
            Description = game.Description,
            RecommendedSystemRequirements = game.RecommendedSystemRequirements,
            PublisherId = game.PublisherId.ToString(),
            DeveloperId = game.DeveloperId.ToString(),
            MainImageUrl = game.MainImageUrl,
            MinSystemRequirements = game.MinSystemRequirements,
            ShortDescription = game.ShortDescription,
        };
        
        return View(dto);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var game = await _gamesRepository.GetEntityByIdAsync(id);
        
        return View(new GameDto
        {
            GameId = game.GameId.ToString(),
            Name = game.Name,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(GameDto dto)
    {
        if (!GuidRegex().IsMatch(dto.DeveloperId) || !GuidRegex().IsMatch(dto.PublisherId) 
                                                  || string.IsNullOrWhiteSpace(dto.Name)
                                                  || string.IsNullOrWhiteSpace(dto.Description)
                                                  || string.IsNullOrWhiteSpace(dto.RecommendedSystemRequirements))
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        await _gamesRepository.AddNewEntityAsync(new Game
        {
            DeveloperId = Guid.Parse(dto.DeveloperId),
            PublisherId = Guid.Parse(dto.PublisherId),
            Name = dto.Name,
            Description = dto.Description,
            MinSystemRequirements = dto.MinSystemRequirements,
            MainImageUrl = dto.MainImageUrl,
            ShortDescription = dto.ShortDescription,
            RecommendedSystemRequirements = dto.RecommendedSystemRequirements,
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(GameDto dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        var game = await _gamesRepository.GetEntityByIdAsync(Guid.Parse(dto.GameId));
        
        if (game is null) 
            return NotFound();
        
        game.DeveloperId = Guid.Parse(dto.DeveloperId);
        game.PublisherId = Guid.Parse(dto.PublisherId);
        game.Name = dto.Name;
        game.MainImageUrl = dto.MainImageUrl;
        game.Description = dto.Description;
        game.ShortDescription = dto.ShortDescription;
        game.MinSystemRequirements = dto.MinSystemRequirements;
        game.RecommendedSystemRequirements = dto.RecommendedSystemRequirements;
        
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
    
    private void PopulateDropdowns()
    {
        ViewData["Publishers"] = new SelectList(
            _publisherRepository.GetAllEntities(), 
            nameof(Publisher.PublisherId),
            nameof(Publisher.PublisherName));

        ViewData["Developers"] = new SelectList(
            _developersRepository.GetAllEntities(),
            nameof(Developer.DeveloperId),
            nameof(Developer.DeveloperName));
    }
}