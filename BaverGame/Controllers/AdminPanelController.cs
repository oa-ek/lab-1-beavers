using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public class AdminPanelController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public AdminPanelController(ILogger<HomeController> logger) => _logger = logger;

    public IActionResult Index() => View();
}