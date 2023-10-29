using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public class AdminPanelController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    public AdminPanelController(ILogger<HomeController> logger) => _logger = logger;

    public IActionResult Index() => View();
}