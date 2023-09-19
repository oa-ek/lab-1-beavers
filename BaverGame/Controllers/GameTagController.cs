using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed partial class GameTagController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<GameTag> _gameTagsRepository;
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    public GameTagController(IRepository<GameTag> gameTagsRepository, ILogger<HomeController> logger)
    {
        _gameTagsRepository = gameTagsRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _gameTagsRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(GameTagDto dto)
    {
        if (!GuidRegex().IsMatch(dto.TagId) || !GuidRegex().IsMatch(dto.GameId))
            return View(dto);
        
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
            return View(dto);
        
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
}