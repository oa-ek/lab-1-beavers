using BaverGame.Application.DTOs;
using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public sealed class StoreController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Store> _storesRepository;
    
    public StoreController(IRepository<Store> storesRepository, ILogger<HomeController> logger)
    {
        _storesRepository = storesRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _storesRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();

    public async Task<IActionResult> Update(Guid id)
    {
        var store = await _storesRepository.GetEntityByIdAsync(id);

        return View(new StoreDto
        {
            StoreId = id.ToString(),
            StoreName = store.StoreName,
            HtmlPriceElements = store.PriceElements,
            MainImageUrl = store.MainImageUrl,
        });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var store = await _storesRepository.GetEntityByIdAsync(id);
        
        return View(new StoreDto
        {
            StoreId = id.ToString(),
            StoreName = store.StoreName,
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(StoreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.StoreName) 
            || string.IsNullOrEmpty(dto.StoreName))
            return View(dto);
        
        await _storesRepository.AddNewEntityAsync(new Store
        {
            StoreName = dto.StoreName,
            PriceElements = dto.HtmlPriceElements,
            MainImageUrl = dto.MainImageUrl,
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(StoreDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var store = await _storesRepository.GetEntityByIdAsync(Guid.Parse(dto.StoreId));
        if (store is null) 
            return NotFound();
        
        store.StoreName = dto.StoreName;
        store.PriceElements = dto.HtmlPriceElements;
        store.MainImageUrl = dto.MainImageUrl;
        _storesRepository.UpdateExistingEntity(store); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(StoreDto dto)
    {
        if (!Guid.TryParse(dto.StoreId, out var id)) return View(dto);
        
        var store = await _storesRepository.GetEntityByIdAsync(id);
        if (store is null) 
            return NotFound();
        
        _storesRepository.RemoveExistingEntity(store); 
        return RedirectToAction("Index");
    }
}