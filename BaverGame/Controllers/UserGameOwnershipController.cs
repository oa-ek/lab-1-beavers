using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed partial class UserGameOwnershipController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<UserGameOwnership> _userGameOwnershipRepository;
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    [GeneratedRegex(RegexPatterns.EmailPattern)]
    private static partial Regex EmailRegex();
    
    public UserGameOwnershipController(IRepository<UserGameOwnership> userGameOwnershipRepository, ILogger<HomeController> logger)
    {
        _userGameOwnershipRepository = userGameOwnershipRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _userGameOwnershipRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(UserGameOwnershipDto dto)
    {
        if (!GuidRegex().IsMatch(dto.UserId) || !EmailRegex().IsMatch(dto.GameId))
            return View(dto);
        
        await _userGameOwnershipRepository.AddNewEntityAsync(new UserGameOwnership
        {
            UserId = Guid.Parse(dto.UserId),
            GameId = Guid.Parse(dto.GameId),
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserGameOwnershipDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var ownership = await _userGameOwnershipRepository.GetEntityByIdAsync(Guid.Parse(dto.GameOwnershipId));
        if (ownership is null) 
            return NotFound();

        ownership.GameId = Guid.Parse(dto.GameId);
        ownership.UserId = Guid.Parse(dto.UserId);
        
        _userGameOwnershipRepository.UpdateExistingEntity(ownership); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(UserGameOwnershipDto dto)
    {
        if (!Guid.TryParse(dto.UserId, out var id)) return View(dto);
        
        var ownership = await _userGameOwnershipRepository.GetEntityByIdAsync(id);
        if (ownership is null) 
            return NotFound();
        
        _userGameOwnershipRepository.RemoveExistingEntity(ownership); 
        return RedirectToAction("Index");
    }
}