using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public class PublisherController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Publisher> _publisherRepository;
    
    public PublisherController(IRepository<Publisher> publisherRepository, ILogger<HomeController> logger)
    {
        _publisherRepository = publisherRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => View(await _publisherRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(PublisherDto publisher)
    {
        if (string.IsNullOrWhiteSpace(publisher.PublisherName) 
            || string.IsNullOrEmpty(publisher.PublisherName))
            return View(publisher);
        
        await _publisherRepository.AddNewEntityAsync(new Publisher
        {
            PublisherName = publisher.PublisherName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(PublisherDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        var publisher = await _publisherRepository.GetEntityByIdAsync(Guid.Parse(dto.PublisherId));

        if (publisher == null) return NotFound();
        
        publisher.PublisherName = dto.PublisherName;
        
        _publisherRepository.UpdateExistingEntity(publisher); 

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PublisherDto dto)
    {
        if (!Guid.TryParse(dto.PublisherId, out var id)) return View(dto);
        
        var publisher = await _publisherRepository.GetEntityByIdAsync(id);

        if (publisher == null) return NotFound();
        
        _publisherRepository.RemoveExistingEntity(publisher); 
        
        return RedirectToAction("Index");
    }
}