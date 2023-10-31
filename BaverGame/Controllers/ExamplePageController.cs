using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaverGame.Controllers;

public sealed class ExamplePageController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<Publisher> _publisherRepository;
    private readonly IRepository<Developer> _developersRepository;

    public ExamplePageController(IRepository<Game> gamesRepository, IRepository<Publisher> publisherRepository, IRepository<Developer> developersRepository, ILogger<HomeController> logger)
    {
        _gamesRepository = gamesRepository;
        _publisherRepository = publisherRepository;
        _developersRepository = developersRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() =>
        View(await _gamesRepository.GetEntityByIdAsync(Guid.Parse("29265adf-a669-477a-ae9b-e5adcd12781a")));
}