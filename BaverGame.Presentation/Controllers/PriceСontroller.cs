using System.Diagnostics;
using System.Text.RegularExpressions;
using BaverGame.Application.DTOs;
using BaverGame.Application.DTOs.ValidationRelated;
using BaverGame.Controllers.Parsing;
using BaverGame.Controllers.Parsing.Core.Services;
using BaverGame.Domain.Contracts;
using BaverGame.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaverGame.Controllers;

[Authorize(Roles = "Administrator")]
public sealed partial class PriceController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepository<Price> _pricesRepository;
    private readonly IRepository<Game> _gamesRepository;
    private readonly IRepository<Store> _storesRepository;

    [GeneratedRegex(RegexPatterns.UrlPattern)]
    private static partial Regex UrlRegex();
    
    [GeneratedRegex(RegexPatterns.GuidPattern)]
    private static partial Regex GuidRegex();
    
    public PriceController(IRepository<Price> pricesRepository, IRepository<Game> gamesRepository, IRepository<Store> storesRepository, ILogger<HomeController> logger)
    {
        _pricesRepository = pricesRepository;
        _gamesRepository = gamesRepository;
        _storesRepository = storesRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index() => 
        View(await _pricesRepository.GetAllEntitiesAsync());
    
    public IActionResult Create()
    {
        PopulateDropdowns();
        return View();
    }

    public async Task<IActionResult> Update(Guid id)
    {
        PopulateDropdowns();
        var price = await _pricesRepository.GetEntityByIdAsync(id);
        var dto = new PriceDto()
        {
            PriceUrl = price.PriceUrl,
            PriceValue = price.PriceValue,
            GameId = price.GameId.ToString(),
            StoreId = price.StoreId.ToString(),
            PriceId = price.PriceId.ToString(),
            CurrencyPostfix = price.CurrencyPostfix,
        };
        return View(dto);
    }

    public async Task<IActionResult> ParseResult()
    {
        Stopwatch stopWatch = new();
        stopWatch.Start();

        List<Price> prices = await _pricesRepository.GetAllEntitiesAsync();
        int priceParsed = 0;
        List<Price> errorPrices = new();
        
        foreach(Price price in prices)
        {
            try
            {
                ParserWorker<decimal> parser = new(
                    new PriceParser(),
                    new PriceParserSettings(price.PriceUrl, price.Store.PriceElements.Split(';')));

                price.PriceValue = await parser.StartAsync();
                priceParsed++;
                _pricesRepository.UpdateExistingEntity(price);
            }
            catch
            {
                errorPrices.Add(price);
            }
        }

        stopWatch.Stop();
        return View(new ParsingPricesResultDto(priceParsed, errorPrices, stopWatch.Elapsed));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var price = await _pricesRepository.GetEntityByIdAsync(id);
        var dto = new PriceDto()
        {
            PriceUrl = price.PriceUrl,
            PriceValue = price.PriceValue,
            GameId = price.GameId.ToString(),
            PriceId = price.PriceId.ToString(),
            StoreId = price.StoreId.ToString(),
            CurrencyPostfix = price.CurrencyPostfix,
        };
        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(PriceDto dto)
    {
        if (!UrlRegex().IsMatch(dto.PriceUrl) || !GuidRegex().IsMatch(dto.GameId)
                                              || !GuidRegex().IsMatch(dto.StoreId)
                                              || string.IsNullOrEmpty(dto.PriceValue.ToString())
                                              || string.IsNullOrWhiteSpace(dto.PriceValue.ToString()))
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        await _pricesRepository.AddNewEntityAsync(new Price
        {
            GameId = Guid.Parse(dto.GameId),
            StoreId = Guid.Parse(dto.StoreId),
            PriceValue = dto.PriceValue,
            PriceUrl = dto.PriceUrl,
            CurrencyPostfix = dto.CurrencyPostfix,
        });

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Update(PriceDto dto)
    {
        if (!ModelState.IsValid)
        {
            PopulateDropdowns();
            return View(dto);
        }
        
        var price = await _pricesRepository.GetEntityByIdAsync(Guid.Parse(dto.PriceId));
        
        if (price is null) 
            return NotFound();

        price.GameId = Guid.Parse(dto.GameId);
        price.StoreId = Guid.Parse(dto.StoreId);
        price.PriceValue = dto.PriceValue;
        price.PriceUrl = dto.PriceUrl;
        price.CurrencyPostfix = dto.CurrencyPostfix;
        
        _pricesRepository.UpdateExistingEntity(price); 
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(PriceDto dto)
    {
        if (!Guid.TryParse(dto.PriceId, out var id)) return View(dto);
        
        var price = await _pricesRepository.GetEntityByIdAsync(id);
        
        if (price is null) 
            return NotFound();
        
        _pricesRepository.RemoveExistingEntity(price); 
        return RedirectToAction("Index");
    }
    
    private void PopulateDropdowns()
    {
        ViewData["Games"] = new SelectList(
            _gamesRepository.GetAllEntities(), 
            nameof(Game.GameId),
            nameof(Game.Name));

        ViewData["Stores"] = new SelectList(
            _storesRepository.GetAllEntities(),
            nameof(Store.StoreId),
            nameof(Store.StoreName));
    }

}