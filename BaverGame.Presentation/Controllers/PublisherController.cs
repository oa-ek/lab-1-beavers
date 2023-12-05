using BaverGame.Application.DTOs;
using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public class PublisherController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Publisher> _publishersRepository;
    
    public PublisherController(IRepository<Publisher> publishersRepository, ILogger<HomeController> logger)
    {
        _publishersRepository = publishersRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => View(await _publishersRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public async Task<IActionResult> Update(Guid id)
    {
        var publisher = await _publishersRepository.GetEntityByIdAsync(id);
        
        return View(new PublisherDto
        {
            PublisherName = publisher.PublisherName,
            PublisherId = id.ToString()
        });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var publisher = await _publishersRepository.GetEntityByIdAsync(id);
        
        return View(new PublisherDto
        {
            PublisherName = publisher.PublisherName,
            PublisherId = id.ToString()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(PublisherDto publisher)
    {
        if (string.IsNullOrWhiteSpace(publisher.PublisherName) 
            || string.IsNullOrEmpty(publisher.PublisherName))
            return View(publisher);
        
        await _publishersRepository.AddNewEntityAsync(new Publisher
        {
            PublisherName = publisher.PublisherName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(PublisherDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        var publisher = await _publishersRepository.GetEntityByIdAsync(Guid.Parse(dto.PublisherId));

        if (publisher == null) return NotFound();
        
        publisher.PublisherName = dto.PublisherName;
        
        _publishersRepository.UpdateExistingEntity(publisher); 

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PublisherDto dto)
    {
        if (!Guid.TryParse(dto.PublisherId, out var id)) return View(dto);
        
        var publisher = await _publishersRepository.GetEntityByIdAsync(id);

        if (publisher == null) return NotFound();
        
        _publishersRepository.RemoveExistingEntity(publisher); 
        
        return RedirectToAction("Index");
    }
}