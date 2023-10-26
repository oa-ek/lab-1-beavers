using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaverGame.Controllers;

public sealed partial class GameTagController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<GameTag> _gameTagsRepository;
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<Tag> _tagsRepository;
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    public GameTagController(
        IRepository<GameTag> gameTagsRepository,
        IRepository<Game> gamesRepository, 
        IRepository<Tag> tagsRepository,
        ILogger<HomeController> logger)
    {
        _gameTagsRepository = gameTagsRepository;
        _logger = logger;
        _gamesRepository = gamesRepository;
        _tagsRepository = tagsRepository;
    }

    public async Task<IActionResult> Index() => 
        View(await _gameTagsRepository.GetAllEntitiesAsync());
    
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    public async Task<IActionResult> Update(Guid id)
    {
        PopulateDropdowns();
        
        var tag = await _gameTagsRepository.GetEntityByIdAsync(id);
        
        return View(new GameTagDto
        {
            GameId = tag.GameId.ToString(),
            TagId = tag.TagId.ToString(),
            GameTagId = id.ToString()
        });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var tag = await _gameTagsRepository.GetEntityByIdAsync(id);
        
        return View(new GameTagDto
        {
            GameTagId = id.ToString()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(GameTagDto dto)
    {
        if (!GuidRegex().IsMatch(dto.TagId) || !GuidRegex().IsMatch(dto.GameId))
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        await _gameTagsRepository.AddNewEntityAsync(new GameTag
        {
            TagId = Guid.Parse(dto.TagId),
            GameId = Guid.Parse(dto.GameId)
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(GameTagDto dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        var tag = await _gameTagsRepository.GetEntityByIdAsync(Guid.Parse(dto.GameTagId));
        
        if (tag is null) 
            return NotFound();
        
        tag.GameId = Guid.Parse(dto.GameId);
        tag.TagId = Guid.Parse(dto.TagId);
        _gameTagsRepository.UpdateExistingEntity(tag); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(GameTagDto dto)
    {
        if (!Guid.TryParse(dto.GameTagId, out var id)) return View(dto);
        
        var tag = await _gameTagsRepository.GetEntityByIdAsync(id);
        if (tag is null) 
            return NotFound();
        
        _gameTagsRepository.RemoveExistingEntity(tag); 
        return RedirectToAction("Index");
    }
    
    private void PopulateDropdowns()
    {
        ViewData["Games"] = new SelectList(
            _gamesRepository.GetAllEntities(), 
            nameof(Game.GameId),
            nameof(Game.Name));

        ViewData["Tags"] = new SelectList(
            _tagsRepository.GetAllEntities(),
            nameof(Tag.TagId),
            nameof(Tag.TagName));
    }
}