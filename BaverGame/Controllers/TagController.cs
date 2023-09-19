using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed class TagController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Tag> _tagsRepository;
    
    public TagController(IRepository<Tag> tagsRepository, ILogger<HomeController> logger)
    {
        _tagsRepository = tagsRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _tagsRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(TagDto dto)
    {
        await _tagsRepository.AddNewEntityAsync(new Tag
        {
            TagName = dto.TagName
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(TagDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var tag = await _tagsRepository.GetEntityByIdAsync(dto.TagId);
        if (tag is null) 
            return NotFound();
        
        tag.TagName = dto.TagName;
        _tagsRepository.UpdateExistingEntity(tag); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(TagDto dto)
    {
        var tag = await _tagsRepository.GetEntityByIdAsync(dto.TagId);
        if (tag is null) 
            return NotFound();
        
        _tagsRepository.RemoveExistingEntity(tag); 
        return RedirectToAction("Index");
    }
}