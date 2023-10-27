using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaverGame.Controllers;

public sealed partial class UserGameOwnershipController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<UserGameOwnership> _userGameOwnershipRepository;
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<User> _usersRepository;

    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    [GeneratedRegex(RegexPatterns.EmailPattern)]
    private static partial Regex EmailRegex();
    
    public UserGameOwnershipController(IRepository<UserGameOwnership> userGameOwnershipRepository,
        IRepository<Game> gamesRepository, IRepository<User> usersRepository, ILogger<HomeController> logger)
    {
        _userGameOwnershipRepository = userGameOwnershipRepository;
        _gamesRepository = gamesRepository;
        _usersRepository = usersRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _userGameOwnershipRepository.GetAllEntitiesAsync());
    
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    public async Task<IActionResult> Update(Guid id)
    {
        PopulateDropdowns();
        var ownership = await _userGameOwnershipRepository.GetEntityByIdAsync(id);
        var dto = new UserGameOwnershipDto()
        {
            GameId = ownership.GameId.ToString(),
            UserId = ownership.UserId.ToString(),
            GameOwnershipId = ownership.OwnershipId.ToString(),
            GameName = ownership.Game.Name,
            UserName = ownership.User.UserName,
        };
        return View(dto);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        PopulateDropdowns();
        var ownership = await _userGameOwnershipRepository.GetEntityByIdAsync(id);
        var dto = new UserGameOwnershipDto()
        {
            GameId = ownership.GameId.ToString(),
            UserId = ownership.UserId.ToString(),
            GameOwnershipId = ownership.OwnershipId.ToString(),
            GameName = ownership.Game.Name,
            UserName = ownership.User.UserName,
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserGameOwnershipDto dto)
    {
        if (!GuidRegex().IsMatch(dto.UserId) || !GuidRegex().IsMatch(dto.GameId))
        {
            PopulateDropdowns();
            return View(dto);
        }
        
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
        {
            PopulateDropdowns();
            return View(dto);
        }
        
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
        if (!Guid.TryParse(dto.GameOwnershipId, out var id)) return View(dto);
        
        var ownership = await _userGameOwnershipRepository.GetEntityByIdAsync(id);
        if (ownership is null) 
            return NotFound();
        
        _userGameOwnershipRepository.RemoveExistingEntity(ownership); 
        return RedirectToAction("Index");
    }
    
    private void PopulateDropdowns()
    {
        ViewData["Games"] = new SelectList(
            _gamesRepository.GetAllEntities(),
            nameof(Game.GameId),
            nameof(Game.Name));
        ViewData["Users"] = new SelectList(
            _usersRepository.GetAllEntities(),
            nameof(Core.User.Id),
            nameof(Core.User.UserName));
    }
}