using System.Text.RegularExpressions;
using BaverGame.DTOs;
using BaverGame.DTOs.ValidationRelated;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed class GameCatalogController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<GameTag> _gameTags;
    private readonly IRepository<Tag> _tags;
    private readonly IRepository<Publisher> _publisherRepository;
    private readonly IRepository<Developer> _developersRepository;
    
    public GameCatalogController(
        IRepository<Game> gamesRepository, IRepository<Publisher> publisherRepository,
        IRepository<Developer> developersRepository, ILogger<HomeController> logger,
        IRepository<GameTag> gameTags, IRepository<Tag> tags)
    {
        _gamesRepository = gamesRepository;
        _publisherRepository = publisherRepository;
        _developersRepository = developersRepository;
        _logger = logger;
        _gameTags = gameTags;
        _tags = tags;
    }

    public async Task<IActionResult> Index(Guid? developerId, Guid? publisherId, Guid? tagId)
    {
        PopulateDropdowns();

        var games = await _gamesRepository.GetAllEntitiesAsync();

        if (developerId.HasValue)
            games = games.Where(game => game.DeveloperId == developerId).ToList();
        
        if (publisherId.HasValue)
            games = games.Where(game => game.PublisherId == publisherId).ToList();
        
        if (tagId.HasValue)
        {
            var tags = await _gameTags.GetAllEntitiesAsync();

            var filteredTags = tags.Where(tag => tag.TagId == tagId).ToList();

            tags = filteredTags;
            
            var result = games.Where(game => tags.SingleOrDefault(
                tag => tag.GameId == game.GameId) is not null).ToList();

            games = result;
        }

        return View(games);
    }

    private void PopulateDropdowns()
    {
        ViewData["Publishers"] = new SelectList(
            _publisherRepository.GetAllEntities(), 
            nameof(Publisher.PublisherId),
            nameof(Publisher.PublisherName));
        
        ViewData["Tags"] = new SelectList(
            _tags.GetAllEntities(), 
            nameof(Tag.TagId),
            nameof(Tag.TagName));

        ViewData["Developers"] = new SelectList(
            _developersRepository.GetAllEntities(),
            nameof(Developer.DeveloperId),
            nameof(Developer.DeveloperName));
    }
}