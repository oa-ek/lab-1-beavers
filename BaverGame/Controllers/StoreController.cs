using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BaverGame.Controllers;

public sealed class StoreController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Store> _storeRepository;
    
    public StoreController(IRepository<Store> storeRepository, ILogger<HomeController> logger)
    {
        _storeRepository = storeRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _storeRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(StoreDto dto)
    {
        await _storeRepository.AddNewEntityAsync(new Store
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
        
        var store = await _storeRepository.GetEntityByIdAsync(dto.StoreId);
        if (store is null) 
            return NotFound();
        
        store.StoreName = dto.StoreName;
        _storeRepository.UpdateExistingEntity(store); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(StoreDto dto)
    {
        var store = await _storeRepository.GetEntityByIdAsync(dto.StoreId);
        if (store is null) 
            return NotFound();
        
        _storeRepository.RemoveExistingEntity(store); 
        return RedirectToAction("Index");
    }
}