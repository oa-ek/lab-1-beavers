using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

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
            StoreName = store.StoreName
        });
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var store = await _storesRepository.GetEntityByIdAsync(id);
        
        return View(new StoreDto
        {
            StoreId = id.ToString(),
            StoreName = store.StoreName
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
            StoreName = dto.StoreName
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