using BaverGame.DTOs;
using Core;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BaverGame.Controllers;

public sealed class PriceController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Price> _pricesRepository;
    
    public PriceController(IRepository<Price> pricesRepository, ILogger<HomeController> logger)
    {
        _pricesRepository = pricesRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _pricesRepository.GetAllEntitiesAsync());
    
    public IActionResult Create() => View();
    
    public IActionResult Update() => View();
    
    public IActionResult Delete() => View();
    
    [HttpPost]
    public async Task<IActionResult> Create(PriceDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);
                
        await _pricesRepository.AddNewEntityAsync(new Price
        {
            GameId = Guid.Parse(dto.GameId),
            StoreId = Guid.Parse(dto.StoreId),
            PriceValue = dto.PriceValue,
            PriceUrl = dto.PriceUrl
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(PriceDto dto)
    {
        if (!ModelState.IsValid) 
            return View(dto);
        
        var price = await _pricesRepository.GetEntityByIdAsync(Guid.Parse(dto.PriceId));
        
        if (price is null) 
            return NotFound();
        
        _pricesRepository.UpdateExistingEntity(price); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PriceDto dto)
    {
        var price = await _pricesRepository.GetEntityByIdAsync(Guid.Parse(dto.PriceId));
        
        if (price is null) 
            return NotFound();
        
        _pricesRepository.RemoveExistingEntity(price); 
        return RedirectToAction("Index");
    }
}